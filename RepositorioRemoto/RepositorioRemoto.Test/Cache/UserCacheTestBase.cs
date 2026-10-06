using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using RepositorioRemoto.Cache;
using RepositorioRemoto.Config;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Test;

public abstract class UserCacheTestBase {
    protected IDistributedCache DistributedCache = null!;
    protected UserCache Cache = null!;

    [SetUp]
    public void SetUp() {
        AppConfig.Configure("dev");
        DistributedCache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
        Cache = new UserCache(DistributedCache);
    }

    protected static User CreateUser(int id, string? name = null) => new(
        id,
        name ?? $"Name{id}",
        $"user{id}",
        $"user{id}@test.com",
        new Address("Street", "Suite", "City", "00000", new Geo("1", "2")),
        "555",
        "site",
        new Company("Company", "phrase", "bs"));
}
