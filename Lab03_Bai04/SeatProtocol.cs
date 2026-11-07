using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Lab02;

namespace Lab03.Bai04;

internal static class SeatClientSettings
{
    public static string Host { get; set; } = "127.0.0.1";
    public static int Port { get; set; } = 8084;
}

internal sealed class SeatMovie
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<int> Rooms { get; set; } = new();
}

internal sealed class SeatSnapshot
{
    public List<SeatMovie> Movies { get; set; } = new();
    public List<string> SoldSeats { get; set; } = new();
    public Dictionary<string, int> SoldByMovie { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public Dictionary<string, decimal> RevenueByMovie { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public string Message { get; set; } = string.Empty;
}

internal sealed class SeatRequest
{
    public string Action { get; set; } = string.Empty;
    public List<SeatMovie>? Movies { get; set; }
    public string? Movie { get; set; }
    public string? Room { get; set; }
    public string? Showtime { get; set; }
    public List<string>? Seats { get; set; }
    public decimal Total { get; set; }
}

internal static class SeatRemote
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public static SeatSnapshot GetSnapshot() => Send(new SeatRequest { Action = "snapshot" });
    public static SeatSnapshot ReplaceMovies(IEnumerable<Lab02.Bai05Movie> movies) => Send(new SeatRequest
    {
        Action = "replace-movies",
        Movies = movies.Select(m => new SeatMovie { Name = m.Name, Price = m.Price, Rooms = m.Rooms.ToList() }).ToList()
    });
    public static SeatSnapshot ReloadInput() => Send(new SeatRequest { Action = "reload-input" });
    public static SeatSnapshot ResetBookings() => Send(new SeatRequest { Action = "reset" });
    public static SeatSnapshot Book(string movie, string room, string showtime, IEnumerable<string> seats, decimal total) => Send(new SeatRequest
    {
        Action = "book", Movie = movie, Room = room, Showtime = showtime, Seats = seats.ToList(), Total = total
    });

    private static SeatSnapshot Send(SeatRequest request)
    {
        using var client = new TcpClient();
        client.Connect(SeatClientSettings.Host, SeatClientSettings.Port);
        using NetworkStream stream = client.GetStream();
        using var writer = new StreamWriter(stream, new UTF8Encoding(false), leaveOpen: true) { AutoFlush = true };
        using var reader = new StreamReader(stream, Encoding.UTF8, false, leaveOpen: true);
        writer.WriteLine(JsonSerializer.Serialize(request, JsonOptions));
        string? response = reader.ReadLine();
        if (string.IsNullOrWhiteSpace(response)) throw new IOException("Server đã ngắt kết nối trước khi phản hồi.");
        SeatSnapshot snapshot = JsonSerializer.Deserialize<SeatSnapshot>(response, JsonOptions) ?? throw new IOException("Phản hồi server không hợp lệ.");
        if (snapshot.Message.StartsWith("ERROR:", StringComparison.Ordinal)) throw new InvalidOperationException(snapshot.Message[6..]);
        return snapshot;
    }
}

internal sealed class SeatTcpServer : IDisposable
{
    private readonly object gate = new();
    private readonly TcpListener listener;
    private readonly CancellationTokenSource cancellation = new();
    private readonly HashSet<string> sold = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, int> counts = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, decimal> revenue = new(StringComparer.OrdinalIgnoreCase);
    private List<SeatMovie> movies = new();

    public SeatTcpServer(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        LoadInput(Path.Combine(AppContext.BaseDirectory, "input5.txt"));
    }

    public void Start() { listener.Start(); _ = AcceptLoopAsync(); }
    public void Stop() { cancellation.Cancel(); listener.Stop(); }

    private async Task AcceptLoopAsync()
    {
        while (!cancellation.IsCancellationRequested)
        {
            try
            {
                TcpClient client = await listener.AcceptTcpClientAsync(cancellation.Token);
                _ = HandleClientAsync(client);
            }
            catch (OperationCanceledException) { break; }
            catch (ObjectDisposedException) { break; }
            catch { if (!cancellation.IsCancellationRequested) await Task.Delay(100, cancellation.Token); }
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        using (client)
        using (NetworkStream stream = client.GetStream())
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        using (var writer = new StreamWriter(stream, new UTF8Encoding(false)) { AutoFlush = true })
        {
            try
            {
                string? line = await reader.ReadLineAsync(cancellation.Token);
                if (line == null) return;
                SeatRequest request = JsonSerializer.Deserialize<SeatRequest>(line) ?? new SeatRequest();
                SeatSnapshot result;
                lock (gate) result = Apply(request);
                await writer.WriteLineAsync(JsonSerializer.Serialize(result));
            }
            catch (Exception ex)
            {
                try { await writer.WriteLineAsync(JsonSerializer.Serialize(new SeatSnapshot { Message = "ERROR:" + ex.Message })); } catch { }
            }
        }
    }

    private SeatSnapshot Apply(SeatRequest request)
    {
        switch (request.Action)
        {
            case "snapshot": break;
            case "reload-input":
                LoadInput(Path.Combine(AppContext.BaseDirectory, "input5.txt"));
                ClearBookings();
                break;
            case "replace-movies":
                movies = (request.Movies ?? new()).Where(m => !string.IsNullOrWhiteSpace(m.Name) && m.Price > 0 && m.Rooms.Count > 0).ToList();
                RemoveInvalidBookings();
                SaveInput(Path.Combine(AppContext.BaseDirectory, "input5.txt"));
                break;
            case "reset": ClearBookings(); break;
            case "book":
                if (request.Movie == null || request.Room == null || request.Showtime == null || request.Seats == null || request.Seats.Count == 0)
                    throw new InvalidOperationException("Thông tin đặt vé chưa đầy đủ.");
                SeatMovie? selected = movies.FirstOrDefault(m => string.Equals(m.Name, request.Movie, StringComparison.OrdinalIgnoreCase));
                if (selected == null || !selected.Rooms.Contains(int.Parse(request.Room, CultureInfo.InvariantCulture))) throw new InvalidOperationException("Phim hoặc phòng chiếu không còn trong danh sách server.");
                var keys = request.Seats.Select(code => request.Movie + "|" + request.Room + "|" + request.Showtime + "|" + code).ToList();
                if (request.Seats.Any(code => !System.Text.RegularExpressions.Regex.IsMatch(code, "^[A-C][1-5]$"))) throw new InvalidOperationException("Mã ghế không hợp lệ.");
                if (keys.Distinct(StringComparer.OrdinalIgnoreCase).Count() != keys.Count) throw new InvalidOperationException("Danh sách ghế có phần tử trùng.");
                if (keys.Any(sold.Contains)) throw new InvalidOperationException("Một hoặc nhiều ghế vừa được client khác đặt. Hãy tải lại trạng thái ghế.");
                decimal rate = request.Showtime switch
                {
                    "09:15" or "11:30" => 0.80m,
                    "18:45" or "20:15" => 1.15m,
                    "21:45" or "23:15" => 0.85m,
                    "14:00" or "16:30" or "Không chọn" => 1.00m,
                    _ => throw new InvalidOperationException("Suất chiếu không hợp lệ.")
                };
                decimal serverTotal = request.Seats.Sum(code => selected.Price * rate * ((code is "A1" or "A5" or "C1" or "C5") ? 0.25m : (code is "B2" or "B3" or "B4") ? 2m : 1m));
                foreach (string key in keys) sold.Add(key);
                counts.TryGetValue(request.Movie, out int count); counts[request.Movie] = count + keys.Count;
                revenue.TryGetValue(request.Movie, out decimal amount); revenue[request.Movie] = amount + serverTotal;
                break;
            default: throw new InvalidOperationException("Lệnh TCP không được hỗ trợ.");
        }
        return Snapshot();
    }

    private void LoadInput(string path)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("Server không tìm thấy input5.txt.", path);
        string[] lines = File.ReadAllLines(path, Encoding.UTF8).Select(s => s.Trim()).Where(s => s.Length > 0).ToArray();
        if (lines.Length == 0 || lines.Length % 3 != 0) throw new InvalidDataException("input5.txt phải có từng nhóm 3 dòng: tên, giá, phòng.");
        var parsed = new List<SeatMovie>();
        for (int i = 0; i < lines.Length; i += 3)
        {
            string digits = new string(lines[i + 1].Where(char.IsDigit).ToArray());
            if (!decimal.TryParse(digits, out decimal price) || price <= 0) throw new InvalidDataException("Giá vé không hợp lệ trong input5.txt.");
            List<int> rooms = System.Text.RegularExpressions.Regex.Matches(lines[i + 2], "[0-9]+")
                .Cast<System.Text.RegularExpressions.Match>().Select(m => int.Parse(m.Value, CultureInfo.InvariantCulture)).Distinct().Order().ToList();
            if (rooms.Count == 0 || rooms.Any(r => r < 1 || r > 99)) throw new InvalidDataException("Phòng chiếu không hợp lệ trong input5.txt.");
            parsed.Add(new SeatMovie { Name = lines[i], Price = price, Rooms = rooms });
        }
        movies = parsed;
    }

    private void SaveInput(string path)
    {
        var lines = movies.SelectMany(m => new[] { m.Name, m.Price.ToString("0", CultureInfo.InvariantCulture), string.Join(", ", m.Rooms) });
        File.WriteAllText(path, string.Join(Environment.NewLine + Environment.NewLine, lines) + Environment.NewLine, new UTF8Encoding(false));
    }

    private void RemoveInvalidBookings()
    {
        HashSet<string> valid = movies.Select(m => m.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        sold.RemoveWhere(key => !valid.Contains(key.Split('|')[0]));
        foreach (string name in counts.Keys.Where(k => !valid.Contains(k)).ToList()) { counts.Remove(name); revenue.Remove(name); }
    }

    private void ClearBookings() { sold.Clear(); counts.Clear(); revenue.Clear(); }
    private SeatSnapshot Snapshot() => new()
    {
        Movies = movies.Select(m => new SeatMovie { Name = m.Name, Price = m.Price, Rooms = m.Rooms.ToList() }).ToList(),
        SoldSeats = sold.ToList(), SoldByMovie = new Dictionary<string, int>(counts, StringComparer.OrdinalIgnoreCase),
        RevenueByMovie = new Dictionary<string, decimal>(revenue, StringComparer.OrdinalIgnoreCase)
    };
    public void Dispose() { Stop(); cancellation.Dispose(); }
}

internal sealed class SeatServerWindow : Form
{
    private readonly SeatTcpServer server;
    public SeatServerWindow(int port)
    {
        Text = "Bài 4 · TCP Server"; StartPosition = FormStartPosition.CenterParent; ClientSize = new Size(520, 190);
        server = new SeatTcpServer(port);
        var title = new Label { Text = "SERVER ĐẶT VÉ · TCP", Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Location = new Point(24, 24) };
        var info = new Label { Text = "Đang lắng nghe 0.0.0.0:" + port + "\r\nDanh sách phim nạp từ input5.txt trên máy chủ. Cho phép TCP qua Windows Firewall.", AutoSize = true, Location = new Point(26, 74) };
        Controls.AddRange(new Control[] { title, info });
        server.Start();
    }
    protected override void OnFormClosed(FormClosedEventArgs e) { server.Dispose(); base.OnFormClosed(e); }
}
