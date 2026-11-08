using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Lab02.Bai06Data;

namespace Lab03.Bai05;

internal sealed class FoodRpcRequest
{
    public string Method { get; set; } = string.Empty;
    public List<string> Arguments { get; set; } = new();
    public string? ImageBase64 { get; set; }
}

internal sealed class FoodRpcResponse
{
    public bool Ok { get; set; }
    public string ResultJson { get; set; } = "null";
    public string Error { get; set; } = string.Empty;
    public long Revision { get; set; }
}

internal sealed class FoodTcpServer : IDisposable
{
    private readonly TcpListener listener;
    private readonly LocalFoodDatabase database = new();
    private readonly CancellationTokenSource stopToken = new();
    private readonly object databaseGate = new();
    private long revision;

    public FoodTcpServer(int port) => listener = new TcpListener(IPAddress.Any, port);
    public void Start() { database.Initialize(); listener.Start(); _ = Task.Run(AcceptLoopAsync); }  // thread pool, không dùng luồng giao diện
    public void Stop() { stopToken.Cancel(); listener.Stop(); }

    private async Task AcceptLoopAsync()
    {
        while (!stopToken.IsCancellationRequested)
        {
            try { _ = HandleAsync(await listener.AcceptTcpClientAsync(stopToken.Token)); }
            catch (OperationCanceledException) { break; }
            catch (ObjectDisposedException) { break; }
            catch { if (!stopToken.IsCancellationRequested) await Task.Delay(100); }
        }
    }

    private async Task HandleAsync(TcpClient client)
    {
        using (client)
        using (NetworkStream stream = client.GetStream())
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        using (var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true })
        {
            try
            {
                string? line = await reader.ReadLineAsync(stopToken.Token);
                if (line == null) return;
                FoodRpcRequest request = JsonSerializer.Deserialize<FoodRpcRequest>(line) ?? throw new InvalidDataException("TCP request trống.");
                FoodRpcResponse response;
                lock (databaseGate) response = Invoke(request);
                await writer.WriteLineAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                try { await writer.WriteLineAsync(JsonSerializer.Serialize(new FoodRpcResponse { Error = ex.GetBaseException().Message })); } catch { }
            }
        }
    }

    private FoodRpcResponse Invoke(FoodRpcRequest request)
    {
        if (request.Method == "__revision")
            return new FoodRpcResponse { Ok = true, ResultJson = JsonSerializer.Serialize(revision), Revision = revision };

        if (request.Method == "__getImage")
        {
            string stored = request.Arguments.Count == 0 ? string.Empty : JsonSerializer.Deserialize<string>(request.Arguments[0]) ?? string.Empty;
            string path = database.ResolveImagePath(stored);
            if (!File.Exists(path)) return new FoodRpcResponse { Ok = true, ResultJson = JsonSerializer.Serialize(string.Empty), Revision = revision };
            return new FoodRpcResponse { Ok = true, ResultJson = JsonSerializer.Serialize(Convert.ToBase64String(File.ReadAllBytes(path))), Revision = revision };
        }

        MethodInfo? method = typeof(LocalFoodDatabase).GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .FirstOrDefault(m => m.Name == request.Method && m.GetParameters().Length == request.Arguments.Count);
        if (method == null) return new FoodRpcResponse { Error = "Không tìm thấy thao tác dữ liệu " + request.Method };
        ParameterInfo[] parameters = method.GetParameters();
        object?[] values = new object?[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
            values[i] = JsonSerializer.Deserialize(request.Arguments[i], parameters[i].ParameterType);

        if (request.Method == nameof(LocalFoodDatabase.AddDish) && request.ImageBase64 != null)
        {
            string fileName = Path.GetFileName((string?)values[1] ?? "dish-image.png");
            if (fileName.Length == 0) fileName = "dish-image.png";
            Directory.CreateDirectory(database.ImageDirectory);
            string imagePath = Path.Combine(database.ImageDirectory, fileName);
            byte[] bytes = Convert.FromBase64String(request.ImageBase64);
            if (bytes.Length > 12 * 1024 * 1024) throw new InvalidDataException("Ảnh vượt quá giới hạn 12 MB.");
            File.WriteAllBytes(imagePath, bytes);
            values[1] = database.RelativeImagePath(fileName);
        }

        object? result = method.Invoke(database, values);
        if (IsWrite(request.Method)) revision++;
        return new FoodRpcResponse { Ok = true, ResultJson = result == null ? "null" : JsonSerializer.Serialize(result, result.GetType()), Revision = revision };
    }

    private static bool IsWrite(string method) => method is "Initialize" or "SeedSampleData" or "EnsureCatalog" or "EnsureExtraCatalog" or "EnsureWorldCatalog" or "CleanupAutoContributors" or "AddContributor" or "DeleteContributor" or "UpdateContributorRole" or "AddDish" or "DeleteDish" or "SaveDishInfo" or "SetFavorite" or "AddHistory" or "ClearHistory" or "AddShop" or "DeleteShop" or "SetSetting";

    public void Dispose() { Stop(); stopToken.Dispose(); }
}

internal sealed class FoodServerWindow : Form
{
    private readonly FoodTcpServer server;

    public FoodServerWindow(int port)
    {
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI", 10F);
        Text = "Bài 5 · TCP Server";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(640, 300);
        MinimumSize = new Size(560, 260);

        server = new FoodTcpServer(port);
        server.Start();   // nếu cổng bận sẽ ném lỗi để launcher báo cho người dùng

        string lan = "(không đọc được)";
        try
        {
            lan = string.Join("   ", System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList
                .Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(a => a.ToString()));
        }
        catch (System.Net.Sockets.SocketException) { }

        Controls.Add(new Label
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24),
            Text = "SERVER MÓN ĂN · TCP/" + port + "\r\n\r\nĐang lắng nghe trên mọi card mạng (0.0.0.0:" + port + ")\r\nClient ở máy khác nhập IP: " + lan + "\r\n\r\nSQLite và ảnh món lưu trong thư mục Data của máy chủ.\r\nCho phép TCP cổng " + port + " qua Windows Firewall (mạng Private)."
        });
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        server.Dispose();
        base.OnFormClosed(e);
    }
}
