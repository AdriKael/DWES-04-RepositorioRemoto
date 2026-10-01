using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using RepositorioRemoto.Cache.Common;
using RepositorioRemoto.Dto;

namespace RepositorioRemoto.Cache;

public class UserCache(IDistributedCache cache) : IUserCache {
    private const string _cacheKey = "users";

    public async Task<User?> GetAsync(int id) {
        var users = await GetUsersAsync();

        return users?.FirstOrDefault(u => u.id == id);
    }

    public async Task AddAsync(User user) {
        var users = await GetUsersAsync() ?? [];

        var existingUser = users.FirstOrDefault(u => u.id == user.id);

        if (existingUser is not null) users.Remove(existingUser);

        users.Add(user);

        await SaveUsersAsync(users);
    }

    public async Task RemoveAsync(int id) {
        var users = await GetUsersAsync();

        if (users is null) return;

        users.RemoveAll(u => u.id == id);

        await SaveUsersAsync(users);
    }

    public async Task ClearAsync() {
        await cache.RemoveAsync(_cacheKey);
    }

    private async Task<List<User>?> GetUsersAsync() {
        var data = await cache.GetAsync(_cacheKey);

        return data is null ? null : JsonSerializer.Deserialize<List<User>>(data);
    }

    private async Task SaveUsersAsync(List<User> users) {
        var data = JsonSerializer.SerializeToUtf8Bytes(users);

        var options = new DistributedCacheEntryOptions {
            AbsoluteExpirationRelativeToNow =
                TimeSpan.FromMinutes(AppConfig.CacheExpirationMinutes)
        };

        await cache.SetAsync(_cacheKey, data, options);
    }
}