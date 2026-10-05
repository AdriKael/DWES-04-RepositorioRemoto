using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using RepositorioRemoto.Api;
using RepositorioRemoto.Cache;
using RepositorioRemoto.Cache.Common;
using RepositorioRemoto.Config;
using RepositorioRemoto.Entities.AppDb;
using RepositorioRemoto.Repositories;
using RepositorioRemoto.Service.Notifications;
using RepositorioRemoto.Service.Synchro;
using RepositorioRemoto.Service.Users;
using RepositorioRemoto.Storages;
using RepositorioRemoto.Validators;
using Serilog;

namespace RepositorioRemoto.Infrastructure;

public abstract class InyectorDependencias {
    private static readonly ILogger _logger = Log.ForContext<InyectorDependencias>();

    public static IServiceProvider BuildServiceProvider(Action<IServiceCollection>? configureAdditional = null) {
        var services = new ServiceCollection();

        Cache(services);
        Validator(services);
        Storage(services);
        Repository(services);
        Notifications(services);
        Api(services);
        Services(services);

        configureAdditional?.Invoke(services);

        return services.BuildServiceProvider();
    }

    private static void Cache(IServiceCollection services) {
        _logger.Debug("[DI] Configurando caché");

        if (AppConfig.Profile.Equals("dev", StringComparison.OrdinalIgnoreCase)) {
            _logger.Debug("[DI] Usando MemoryDistributedCache");

            services.AddDistributedMemoryCache();
        }
        else {
            _logger.Debug("[DI] Usando Redis");

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = AppConfig.RedisConnection;
                options.InstanceName = "RepositorioRemoto:";
            });
        }

        services.AddSingleton<IUserCache, UserCache>();
    }

    private static void Validator(IServiceCollection services) {
        _logger.Debug("[DI] Creando validador");

        services.AddTransient<IUserValidator, UserValidator>();
    }

    private static void Storage(IServiceCollection services) {
        _logger.Debug("[DI] Creando storage");

        services.AddTransient<IUserStorage, UserStorage>();
    }

    private static void Repository(IServiceCollection services) {
        _logger.Debug("[DI] Configurando repositorio para {Profile}", AppConfig.Profile);

        if (AppConfig.Profile.Equals("dev", StringComparison.OrdinalIgnoreCase)) {
            _logger.Debug("[DI] Usando SQLite");

            services.AddDbContext<AppDbContextSqlite>(options => { options.UseSqlite(AppConfig.SqliteConnection); });

            services.AddScoped<IAppDbContext>(sp =>
                sp.GetRequiredService<AppDbContextSqlite>());
        }
        else {
            _logger.Debug("[DI] Usando PostgreSQL");

            services.AddDbContext<AppDbContextPostgre>(options => {
                options.UseNpgsql(AppConfig.PostgreSqlConnection);
            });

            services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContextPostgre>());
        }

        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void Notifications(IServiceCollection services) {
        _logger.Debug("[DI] Creando servicio de notificaciones");

        services.AddSingleton<INotificationService, NotificationService>();
    }

    private static void Api(IServiceCollection services) {
        _logger.Debug("[DI] Creando cliente de JSONPlaceholder");

        services
            .AddRefitClient<IJsonPlaceholderApi>()
            .ConfigureHttpClient(client => { client.BaseAddress = new Uri(AppConfig.ApiBaseUrl); });
    }

    private static void Services(IServiceCollection services) {
        _logger.Debug("[DI] Creando servicios");

        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<SynchroService>();
    }
}