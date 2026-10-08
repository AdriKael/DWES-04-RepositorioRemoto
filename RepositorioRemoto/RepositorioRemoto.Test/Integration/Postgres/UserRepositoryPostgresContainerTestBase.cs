using Microsoft.EntityFrameworkCore;
using RepositorioRemoto.Entities.AppDb;
using RepositorioRemoto.Models;
using RepositorioRemoto.Repositories;
using Testcontainers.PostgreSql;

namespace RepositorioRemoto.Test.Integration;

/// <summary>
/// Base común para los tests de integración de <see cref="UserRepository"/>
/// con PostgreSQL real (Testcontainers). Gestiona el contenedor efímero
/// (uno por fixture) y un contexto fresco por test.
/// </summary>
public abstract class UserRepositoryPostgresContainerTestBase
{
    private PostgreSqlContainer _container = null!;
    protected AppDbContextPostgre Context = null!;
    protected UserRepository Repository = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _container = new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("test_db")
            .WithUsername("test")
            .WithPassword("test")
            .Build();

        await _container.StartAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _container.DisposeAsync();
    }

    [SetUp]
    public async Task SetUp()
    {
        var options = new DbContextOptionsBuilder<AppDbContextPostgre>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        Context = new AppDbContextPostgre(options);
        await Context.Database.EnsureCreatedAsync();

        Repository = new UserRepository(Context);
    }

    [TearDown]
    public async Task TearDown()
    {
        await Context.Database.EnsureDeletedAsync();
        await Context.DisposeAsync();
    }

    protected static User CreateUser(int id) => new(
        id,
        $"Name{id}",
        $"user{id}",
        $"user{id}@test.com",
        new Address("Street", "Suite", "City", "00000", new Geo("1", "2")),
        "555",
        "site",
        new Company("Company", "phrase", "bs"));
}
