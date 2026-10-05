using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using RepositorioRemoto.Cache.Common;
<<<<<<< HEAD
using RepositorioRemoto.Dto;
=======
using RepositorioRemoto.Config;
using RepositorioRemoto.Models;
using Serilog;
>>>>>>> feat/cache

namespace RepositorioRemoto.Cache;

public class UserCache(IDistributedCache cache) : IUserCache {
    private const string _cacheKey = "users";
<<<<<<< HEAD

    public async Task<User?> GetAsync(int id) {
        var users = await GetUsersAsync();

        return users?.FirstOrDefault(u => u.id == id);
    }

    public async Task AddAsync(User user) {
        var users = await GetUsersAsync() ?? [];

        var existingUser = users.FirstOrDefault(u => u.id == user.id);
=======
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
>>>>>>> feat/cache

        if (existingUser is not null) users.Remove(existingUser);

        users.Add(user);

        await SaveUsersAsync(users);
    }

    public async Task RemoveAsync(int id) {
<<<<<<< HEAD
=======
        _logger.Debug("[CACHE-DEL] Eliminando Usuario");
>>>>>>> feat/cache
        var users = await GetUsersAsync();

        if (users is null) return;

<<<<<<< HEAD
        users.RemoveAll(u => u.id == id);
=======
        users.RemoveAll(u => u.Id == id);
>>>>>>> feat/cache

        await SaveUsersAsync(users);
    }

    public async Task ClearAsync() {
<<<<<<< HEAD
=======
        _logger.Debug("[CACHE-DEL] Limpiando Caché");
>>>>>>> feat/cache
        await cache.RemoveAsync(_cacheKey);
    }

    private async Task<List<User>?> GetUsersAsync() {
<<<<<<< HEAD
=======
        _logger.Debug("[CACHE-GET] Deserializando Usuario");
>>>>>>> feat/cache
        var data = await cache.GetAsync(_cacheKey);

        return data is null ? null : JsonSerializer.Deserialize<List<User>>(data);
    }

    private async Task SaveUsersAsync(List<User> users) {
<<<<<<< HEAD
=======
        _logger.Debug("[CACHE-ADD] Serializando Usuario");
>>>>>>> feat/cache
        var data = JsonSerializer.SerializeToUtf8Bytes(users);

        var options = new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow =
                TimeSpan.FromMinutes(AppConfig.CacheExpirationMinutes)
        };

        await cache.SetAsync(_cacheKey, data, options);
    }
}