using Microsoft.Extensions.DependencyInjection;
using RepositorioRemoto.Api;
using RepositorioRemoto.Cache.Common;
using RepositorioRemoto.Config;
using RepositorioRemoto.Repositories;
using Serilog;

namespace RepositorioRemoto.Service.Synchro;

public class SynchroService(
    IServiceProvider provider,
    IUserCache cache) {
    private static readonly ILogger _logger = Log.ForContext<SynchroService>();

    public async Task StartAsync(
        CancellationToken cancellationToken = default) {
        _logger.Debug("[SYNC-START] Iniciando sincronización cada {Seconds} segundos", AppConfig.SyncIntervalSeconds);
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(AppConfig.SyncIntervalSeconds));

        while (await timer.WaitForNextTickAsync(cancellationToken)) {
            _logger.Debug("[SYNC-START] Comenzando nueva sincronización");
            using var scope = provider.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var api = scope.ServiceProvider.GetRequiredService<IJsonPlaceholderApi>();

            _logger.Debug("[SYNC-CACHE] Limpiando caché");
            await cache.ClearAsync();
            _logger.Debug("[SYNC-DB] Eliminando usuarios de la base de datos");
            await repository.DeleteAllAsync();

            _logger.Debug("[SYNC-API] Obteniendo usuarios desde JSONPlaceholder");
            var users = await api.GetUsuariosAsync();

            _logger.Debug("[SYNC-API] Se han obtenido {Count} usuarios", users.Count);
            foreach (var user in users)
                await repository.CreateAsync(user);

            _logger.Debug("[SYNC-END] Sincronización completada. {Count} usuarios cargados", users.Count);
        }
    }
}