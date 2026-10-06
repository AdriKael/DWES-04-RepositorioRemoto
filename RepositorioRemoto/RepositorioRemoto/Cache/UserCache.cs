using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using RepositorioRemoto.Cache.Common;
using RepositorioRemoto.Config;
using RepositorioRemoto.Models;
using Serilog;

namespace RepositorioRemoto.Cache;

public class UserCache(IDistributedCache cache) : IUserCache {
    private const string _cacheKey = "users";
    private static readonly ILogger _logger = Log.ForContext<UserCache>();

    public async Task<User?> GetAsync(int id) {
        _logger.Debug("[CACHE-GET] Obteniendo Usuario");
        var users = await GetUsersAsync();

        return users?.FirstOrDefault(u => u.Id == id);
    }

    public async Task AddAsync(User user) {
        _logger.Debug("[CACHE-ADD] Añadiendo Usuario");
        var users = await GetUsersAsync() ?? [];

        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);

        if (existingUser is not null) users.Remove(existingUser);

        users.Add(user);

        await SaveUsersAsync(users);
    }

    public async Task RemoveAsync(int id) {
        _logger.Debug("[CACHE-DEL] Eliminando Usuario");
        var users = await GetUsersAsync();

        if (users is null) return;

        users.RemoveAll(u => u.Id == id);

        await SaveUsersAsync(users);
    }

    public async Task ClearAsync() {
        _logger.Debug("[CACHE-DEL] Limpiando Caché");
        await cache.RemoveAsync(_cacheKey);
    }

    private async Task<List<User>?> GetUsersAsync() {
        _logger.Debug("[CACHE-GET] Deserializando Usuario");
        var data = await cache.GetAsync(_cacheKey);

        return data is null ? null : JsonSerializer.Deserialize<List<User>>(data);
    }

    private async Task SaveUsersAsync(List<User> users) {
        _logger.Debug("[CACHE-ADD] Serializando Usuario");
        var data = JsonSerializer.SerializeToUtf8Bytes(users);

        var options = new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow =
                TimeSpan.FromMinutes(AppConfig.CacheExpirationMinutes)
        };

        await cache.SetAsync(_cacheKey, data, options);
    }
}