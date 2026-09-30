using Microsoft.Extensions.Configuration;

public abstract record AppConfig {
    static AppConfig() {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", false, true)
            .Build();
    }

    private static IConfiguration Configuration { get; }

    public static string Profile =>
        Configuration.GetValue<string>("AppConfig:Profile") ?? "Dev";

    public static string SqliteConnection =>
        Configuration.GetValue<string>("Repository:SqliteConnection")
        ?? "Data Source=users.db";

    public static string PostgreSqlConnection =>
        Configuration.GetValue<string>("Repository:PostgreSqlConnection")
        ?? "Host=localhost;Port=5432;Database=users;Username=postgres;Password=postgres";

    public static string RedisConnection =>
        Configuration.GetValue<string>("Cache:RedisConnection")
        ?? "localhost:6379";

    public static int CacheExpirationMinutes =>
        Configuration.GetValue("Cache:ExpirationMinutes", 10);

    public static string ApiBaseUrl =>
        Configuration.GetValue<string>("Api:BaseUrl")
        ?? "https://jsonplaceholder.typicode.com/";

    public static int SyncIntervalSeconds =>
        Configuration.GetValue("Sync:IntervalSeconds", 60);
}