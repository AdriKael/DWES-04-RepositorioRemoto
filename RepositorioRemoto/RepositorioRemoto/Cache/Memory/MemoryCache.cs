using Microsoft.Extensions.Caching.Memory;
using RepositorioRemoto.Cache.Common;

namespace RepositorioRemoto.Cache.Memory;

public class MemoryCache<T>(IMemoryCache cache) : ICache<T> {
    private readonly HashSet<string> _keys = [];

    public Task<T?> GetAsync(string key) {
        cache.TryGetValue(key, out T? value);

        return Task.FromResult(value);
    }

    public Task SetAsync(string key, T value, TimeSpan? expiration = null) {
        var options = new MemoryCacheEntryOptions();

        if (expiration.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiration.Value;

        cache.Set(key, value, options);
        _keys.Add(key);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key) {
        cache.Remove(key);
        _keys.Remove(key);

        return Task.CompletedTask;
    }

    public Task ClearAsync() {
        foreach (var key in _keys)
            cache.Remove(key);

        _keys.Clear();
        return Task.CompletedTask;
    }
}