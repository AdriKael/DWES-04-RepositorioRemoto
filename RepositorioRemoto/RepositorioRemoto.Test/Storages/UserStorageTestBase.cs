using RepositorioRemoto.Models;
using RepositorioRemoto.Storages;

namespace RepositorioRemoto.Test;

public abstract class UserStorageTestBase {
    protected UserStorage Storage = null!;
    protected string TestDirectory = null!;

    [SetUp]
    public void SetUp() {
        Storage = new UserStorage();
        TestDirectory = Path.Combine(Path.GetTempPath(), "RepositorioRemotoTests", Guid.NewGuid().ToString("N"));
    }

    [TearDown]
    public void TearDown() {
        if (Directory.Exists(TestDirectory)) Directory.Delete(TestDirectory, true);
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
