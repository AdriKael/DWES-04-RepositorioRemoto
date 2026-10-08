using FluentAssertions;
using RepositorioRemoto.Cache;

namespace RepositorioRemoto.Test.Integration;

/// <summary>
/// Casos incorrectos / borde de <see cref="UserCache"/> con Redis real.
/// </summary>
[TestFixture]
public class UserCacheRedisContainerCasosIncorrectosTest : UserCacheRedisContainerTestBase
{
    [Test]
    public async Task Get_NonExistingUser_ReturnsNull()
    {
        // Act
        var resultado = await Cache.GetAsync(9999);

        // Assert
        resultado.Should().BeNull();
    }

    [Test]
    public async Task Remove_NonExistingUser_KeepsExisting()
    {
        // Arrange
        await Cache.AddAsync(CreateUser(1));

        // Act
        await Cache.RemoveAsync(9999);

        // Assert
        (await Cache.GetAsync(1)).Should().NotBeNull();
    }
}
