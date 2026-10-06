using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RepositorioRemoto.Entities.AppDb;
using RepositorioRemoto.Models;
using RepositorioRemoto.Repositories;

namespace RepositorioRemoto.Test;

public abstract class UserRepositoryTestBase {
    protected SqliteConnection Connection = null!;
    protected AppDbContextSqlite Context = null!;
    protected UserRepository Repository = null!;

    [SetUp]
    public void SetUp() {
        Connection = new SqliteConnection("Data Source=:memory:");
        Connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContextSqlite>()
            .UseSqlite(Connection)
            .Options;
        Context = new AppDbContextSqlite(options);
        Repository = new UserRepository(Context);
    }

    [TearDown]
    public void TearDown() {
        Context.Dispose();
        Connection.Dispose();
    }

    protected async Task AddUserAsync(User user) {
        Context.Users.Add(user);
        await Context.SaveChangesAsync();
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
