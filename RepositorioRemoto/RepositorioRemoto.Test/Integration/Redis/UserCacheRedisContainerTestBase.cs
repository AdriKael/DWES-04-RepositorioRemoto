using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using RepositorioRemoto.Cache;
using RepositorioRemoto.Config;
using RepositorioRemoto.Models;
using Testcontainers.Redis;

namespace RepositorioRemoto.Test.Integration;

/// <summary>
/// Base común para los tests de integración de <see cref="UserCache"/>
/// con Redis real (Testcontainers). Gestiona el contenedor efímero
/// (uno por fixture) y una caché Redis fresca por test.
/// </summary>
public abstract class UserCacheRedisContainerTestBase
{
    private RedisContainer _container = null!;
    private ServiceProvider _serviceProvider = null!;
    protected IDistributedCache DistributedCache = null!;
    protected UserCache Cache = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _container = new RedisBuilder("redis:7-alpine").Build();
        await _container.StartAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _container.DisposeAsync();
    }

    [SetUp]
    public void SetUp()
    {
        AppConfig.Configure("dev");

        var services = new ServiceCollection();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = _container.GetConnectionString();
            options.InstanceName = "Test:";
        });

        _serviceProvider = services.BuildServiceProvider();
        DistributedCache = _serviceProvider.GetRequiredService<IDistributedCache>();
        Cache = new UserCache(DistributedCache);
    }

    [TearDown]
    public async Task TearDown()
    {
        await DistributedCache.RemoveAsync("users");
        await _serviceProvider.DisposeAsync();
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
