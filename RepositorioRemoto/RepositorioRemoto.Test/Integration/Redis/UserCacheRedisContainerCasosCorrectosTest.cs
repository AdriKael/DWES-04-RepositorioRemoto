using FluentAssertions;
using RepositorioRemoto.Cache;

namespace RepositorioRemoto.Test.Integration;

/// <summary>
/// Casos correctos de <see cref="UserCache"/> con Redis real.
/// </summary>
[TestFixture]
public class UserCacheRedisContainerCasosCorrectosTest : UserCacheRedisContainerTestBase
{
    [Test]
    public async Task Add_NewUser_AllowsRetrieval()
    {
        // Arrange
        var user = CreateUser(1);

        // Act
        await Cache.AddAsync(user);

        // Assert
        (await Cache.GetAsync(1)).Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task Add_SameId_ReplacesPrevious()
    {
        // Arrange
        await Cache.AddAsync(CreateUser(1, "Old"));
        var updated = CreateUser(1, "Updated");

        // Act
        await Cache.AddAsync(updated);

        // Assert
        (await Cache.GetAsync(1)).Should().BeEquivalentTo(updated);
    }

    [Test]
    public async Task Remove_ExistingUser_CannotBeRetrieved()
    {
        // Arrange
        await Cache.AddAsync(CreateUser(1));

        // Act
        await Cache.RemoveAsync(1);

        // Assert
        (await Cache.GetAsync(1)).Should().BeNull();
    }

    [Test]
    public async Task Remove_WithOtherUsers_KeepsNonDeleted()
    {
        // Arrange
        await Cache.AddAsync(CreateUser(1));
        await Cache.AddAsync(CreateUser(2));

        // Act
        await Cache.RemoveAsync(1);

        // Assert
        (await Cache.GetAsync(1)).Should().BeNull();
        (await Cache.GetAsync(2)).Should().NotBeNull();
    }

    [Test]
    public async Task Clear_WithUsers_CleansAllCache()
    {
        // Arrange
        await Cache.AddAsync(CreateUser(1));
        await Cache.AddAsync(CreateUser(2));

        // Act
        await Cache.ClearAsync();

        // Assert
        (await Cache.GetAsync(1)).Should().BeNull();
        (await Cache.GetAsync(2)).Should().BeNull();
    }
}
