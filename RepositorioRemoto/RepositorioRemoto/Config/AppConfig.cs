using Microsoft.Extensions.Configuration;

namespace RepositorioRemoto.Config;

public abstract record AppConfig {
    private static IConfiguration Configuration { get; set; } = null!;

    public static string Profile => Configuration.GetValue<string>("AppConfig:Profile") ?? "dev";

    public static string SqliteConnection {
        get {
            var dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");

            Directory.CreateDirectory(dataFolder);

            var databasePath = Path.Combine(dataFolder, "users.db");

            return $"Data Source={databasePath}";
        }
    }

    public static string PostgreSqlConnection =>
        Configuration.GetValue<string>("Repository:PostgreSqlConnection") ?? "";

    public static string RedisConnection => Configuration.GetValue<string>("Cache:RedisConnection") ?? "";

    public static int CacheExpirationMinutes => Configuration.GetValue("Cache:ExpirationMinutes", 10);

    public static string ApiBaseUrl =>
        Configuration.GetValue<string>("Api:BaseUrl") ?? "https://jsonplaceholder.typicode.com/";

    public static int SyncIntervalSeconds => Configuration.GetValue("Sync:IntervalSeconds", 60);

    private static string DataFolder => Path.Combine(Directory.GetCurrentDirectory(), "data");

    public static string UsersJsonPath => Path.Combine(DataFolder, "users.json");

    public static void Configure(string profile) {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile($"appsettings.{profile}.json", false, true)
            .Build();
    }
}