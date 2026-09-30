using System.Text.Json;
using RepositorioRemoto.Cache.Common;
using StackExchange.Redis;

namespace RepositorioRemoto.Cache.Redis;

public class RedisCache<T>(IConnectionMultiplexer redis) : ICache<T> {
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<T?> GetAsync(string key) {
        var value = await _database.StringGetAsync(key);

        return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(((string)value)!);
    }

    public async Task SetAsync(string key, T value, TimeSpan? expiration = null) {
        var serializedValue = JsonSerializer.Serialize(value);

        if (expiration != null)
            await _database.StringSetAsync(key, serializedValue, (Expiration)expiration);
    }

    public async Task RemoveAsync(string key) {
        await _database.KeyDeleteAsync(key);
    }

    public async Task ClearAsync() {
        var endpoints = _database.Multiplexer.GetEndPoints();

        foreach (var endpoint in endpoints) {
            var server = _database.Multiplexer.GetServer(endpoint);

            var keys = server.Keys();

            foreach (var key in keys)
                await _database.KeyDeleteAsync(key);
        }
    }
}