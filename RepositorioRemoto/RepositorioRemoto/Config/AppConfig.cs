using Microsoft.Extensions.Configuration;

public abstract record AppConfig {
    static AppConfig() {
        var profile = Environment.GetEnvironmentVariable("APP_PROFILE") ?? "dev";

        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile($"appsettings.{profile}.json", false, true)
            .Build();
    }

    private static IConfiguration Configuration { get; }

    public static string Profile => Configuration.GetValue<string>("Profile") ?? "dev";

    public static string SqliteConnection =>
        Configuration.GetValue<string>("Repository:SqliteConnection") ?? "Data Source=users.db";

    public static string PostgreSqlConnection =>
        Configuration.GetValue<string>("Repository:PostgreSqlConnection") ?? "";

    public static string RedisConnection => Configuration.GetValue<string>("Cache:RedisConnection") ?? "";

    public static int CacheExpirationMinutes => Configuration.GetValue("Cache:ExpirationMinutes", 10);

    public static string ApiBaseUrl =>
        Configuration.GetValue<string>("Api:BaseUrl") ?? "https://jsonplaceholder.typicode.com/";

    public static int SyncIntervalSeconds => Configuration.GetValue("Sync:IntervalSeconds", 60);
}