using System.Reflection;
using System.Net.Sockets;
using System.Text.Json;

namespace Lab02.Bai06Data;

internal static class FoodNetworkOptions
{
    public static string Host { get; set; } = "127.0.0.1";
    public static int Port { get; set; } = 8085;
    public static bool IsClient { get; set; }
}

internal sealed class FoodDatabase
{
    private readonly LocalFoodDatabase? local;
    private readonly RemoteFoodClient? remote;
    private readonly string clientDataDirectory = string.Empty;

    public FoodDatabase()
    {
        if (FoodNetworkOptions.IsClient)
        {
            clientDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NT106", "Lab03_Bai05_Client");
            Directory.CreateDirectory(Path.Combine(clientDataDirectory, "CustomImages"));
            remote = new RemoteFoodClient(Path.Combine(clientDataDirectory, "CustomImages"));
        }
        else local = new LocalFoodDatabase();
    }

    public string DataDirectory => local?.DataDirectory ?? clientDataDirectory;
    public string ImageDirectory => local?.ImageDirectory ?? Path.Combine(clientDataDirectory, "CustomImages");
    public string DatabasePath => local?.DatabasePath ?? "SQLite tập trung trên server " + FoodNetworkOptions.Host + ":" + FoodNetworkOptions.Port;
    public string LastSql => local?.LastSql ?? "TCP → SQLite server";
    public long Revision => remote?.GetRevision() ?? 0;

    private T Call<T>(string name, params object[] args)
    {
        if (remote != null) return remote.Call<T>(name, args);
        MethodInfo method = local!.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).First(m => m.Name == name && m.GetParameters().Length == args.Length);
        object? result;
        try { result = method.Invoke(local, args); }
        catch (TargetInvocationException ex) when (ex.InnerException != null) { throw ex.InnerException; }
        return result == null ? default! : (T)result;
    }

    private void CallVoid(string name, params object[] args) { _ = Call<object?>(name, args); }

    public string RelativeImagePath(string fileName) => local?.RelativeImagePath(fileName) ?? Path.Combine("Images", fileName);
    public string ResolveImagePath(string storedPath) => remote?.DownloadImage(storedPath) ?? Call<string>(nameof(ResolveImagePath), storedPath);
    public void Initialize() => CallVoid(nameof(Initialize));
    public void SeedSampleData() => CallVoid(nameof(SeedSampleData));
    public int EnsureCatalog() => Call<int>(nameof(EnsureCatalog));
    public int EnsureExtraCatalog() => Call<int>(nameof(EnsureExtraCatalog));
    public int EnsureWorldCatalog() => Call<int>(nameof(EnsureWorldCatalog));
    public int CleanupAutoContributors() => Call<int>(nameof(CleanupAutoContributors));
    public int GetSystemOwnerId() => Call<int>(nameof(GetSystemOwnerId));
    public int CountDishesOf(int contributorId) => Call<int>(nameof(CountDishesOf), contributorId);
    public int CountContributors() => Call<int>(nameof(CountContributors));
    public int CountDishes() => Call<int>(nameof(CountDishes));
    public int CountDishesByMeal(string meal) => Call<int>(nameof(CountDishesByMeal), meal);
    public List<Contributor> GetContributors() => Call<List<Contributor>>(nameof(GetContributors));
    public int AddContributor(string fullName, string role) => Call<int>(nameof(AddContributor), fullName, role);
    public bool DeleteContributor(int contributorId, bool moveDishesToLibrary) => Call<bool>(nameof(DeleteContributor), contributorId, moveDishesToLibrary);
    public void UpdateContributorRole(int contributorId, string role) => CallVoid(nameof(UpdateContributorRole), contributorId, role);
    public List<Dish> GetDishes(int contributorId) => Call<List<Dish>>(nameof(GetDishes), contributorId);
    public List<Dish> QueryDishes(int contributorId, string category, string meal, int maxPrice) => Call<List<Dish>>(nameof(QueryDishes), contributorId, category, meal, maxPrice);
    public List<Dish> GetSuggestions(string category, string meal, int maxPrice, int count) => Call<List<Dish>>(nameof(GetSuggestions), category, meal, maxPrice, count);
    public Dish GetRandomDish() => Call<Dish>(nameof(GetRandomDish));
    public int GetMinPrice(string category, string meal) => Call<int>(nameof(GetMinPrice), category, meal);
    public Dish GetRandomDish(string category, string meal, int maxPrice) => Call<Dish>(nameof(GetRandomDish), category, meal, maxPrice);
    public Dish GetDishById(int dishId) => Call<Dish>(nameof(GetDishById), dishId);
    public Dish FindDishByName(string name) => Call<Dish>(nameof(FindDishByName), name);
    public int AddDish(string dishName, string imagePath, int contributorId) => Call<int>(nameof(AddDish), dishName, imagePath, contributorId);
    public void DeleteDish(int dishId) => CallVoid(nameof(DeleteDish), dishId);
    public void SaveDishInfo(int dishId, string category, string meals, int price, string region, string description) => CallVoid(nameof(SaveDishInfo), dishId, category, meals, price, region, description);
    public List<ContributorStat> GetContributorStats() => Call<List<ContributorStat>>(nameof(GetContributorStats));
    public List<CategoryStat> GetCategoryStats() => Call<List<CategoryStat>>(nameof(GetCategoryStats));
    public HashSet<int> GetFavoriteIds() => Call<HashSet<int>>(nameof(GetFavoriteIds));
    public List<Dish> GetFavorites() => Call<List<Dish>>(nameof(GetFavorites));
    public void SetFavorite(int dishId, bool favorite) => CallVoid(nameof(SetFavorite), dishId, favorite);
    public void AddHistory(int dishId, string source) => CallVoid(nameof(AddHistory), dishId, source);
    public List<HistoryEntry> GetHistory(int limit) => Call<List<HistoryEntry>>(nameof(GetHistory), limit);
    public void ClearHistory() => CallVoid(nameof(ClearHistory));
    public List<ShopInfo> GetShops() => Call<List<ShopInfo>>(nameof(GetShops));
    public int AddShop(string name, string address, int dishId, string note) => Call<int>(nameof(AddShop), name, address, dishId, note);
    public void DeleteShop(int shopId) => CallVoid(nameof(DeleteShop), shopId);
    public string GetSetting(string key, string fallback) => Call<string>(nameof(GetSetting), key, fallback);
    public void SetSetting(string key, string value) => CallVoid(nameof(SetSetting), key, value);
}

internal sealed class RemoteFoodClient
{
    private readonly string imageDirectory;
    private long revision;
    public RemoteFoodClient(string imageDirectory) => this.imageDirectory = imageDirectory;

    public long GetRevision() => Call<long>("__revision", Array.Empty<object>());

    public T Call<T>(string method, object[] args)
    {
        var request = new Lab03.Bai05.FoodRpcRequest
        {
            Method = method,
            Arguments = args.Select(arg => JsonSerializer.Serialize(arg, arg?.GetType() ?? typeof(object))).ToList()
        };
        if (method == "AddDish" && args.Length > 1 && args[1] is string imagePath)
        {
            string candidate = Path.IsPathRooted(imagePath) ? imagePath : Path.Combine(imageDirectory, Path.GetFileName(imagePath));
            if (File.Exists(candidate)) request.ImageBase64 = Convert.ToBase64String(File.ReadAllBytes(candidate));
        }
        Lab03.Bai05.FoodRpcResponse response = Send(request);
        if (!response.Ok) throw new InvalidOperationException(response.Error);
        revision = response.Revision;
        if (string.IsNullOrWhiteSpace(response.ResultJson) || response.ResultJson == "null") return default!;
        return JsonSerializer.Deserialize<T>(response.ResultJson)!
            ?? throw new InvalidDataException("Server trả về dữ liệu không đúng định dạng.");
    }

    public string DownloadImage(string storedPath)
    {
        if (string.IsNullOrWhiteSpace(storedPath)) return string.Empty;
        var response = Send(new Lab03.Bai05.FoodRpcRequest { Method = "__getImage", Arguments = new List<string> { JsonSerializer.Serialize(storedPath) } });
        if (!response.Ok) throw new InvalidOperationException(response.Error);
        string? encoded = JsonSerializer.Deserialize<string>(response.ResultJson);
        if (string.IsNullOrWhiteSpace(encoded)) return string.Empty;
        string fileName = Path.GetFileName(storedPath);
        if (string.IsNullOrWhiteSpace(fileName)) fileName = "server-image.png";
        string target = Path.Combine(imageDirectory, fileName);
        File.WriteAllBytes(target, Convert.FromBase64String(encoded));
        return target;
    }

    private Lab03.Bai05.FoodRpcResponse Send(Lab03.Bai05.FoodRpcRequest request)
    {
        using var client = new System.Net.Sockets.TcpClient();
        client.Connect(FoodNetworkOptions.Host, FoodNetworkOptions.Port);
        using NetworkStream stream = client.GetStream();
        using var writer = new StreamWriter(stream, new System.Text.UTF8Encoding(false), leaveOpen: true) { AutoFlush = true };
        using var reader = new StreamReader(stream, System.Text.Encoding.UTF8, false, leaveOpen: true);
        writer.WriteLine(JsonSerializer.Serialize(request));
        string? response = reader.ReadLine();
        if (string.IsNullOrWhiteSpace(response)) throw new IOException("Server đã đóng kết nối trước khi phản hồi.");
        return JsonSerializer.Deserialize<Lab03.Bai05.FoodRpcResponse>(response) ?? throw new InvalidDataException("Phản hồi server không hợp lệ.");
    }
}
