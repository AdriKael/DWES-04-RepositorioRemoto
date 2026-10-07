using FluentAssertions;

namespace RepositorioRemoto.Test.Cache;

[TestFixture]
public class UserCacheCasosIncorrectosTest : UserCacheTestBase {
    [Test]
    public async Task GetAsync_CacheVacia_RetornaNull() {
        // Arrange

        // Act
        var result = await Cache.GetAsync(99);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task AddAsync_UsuarioNulo_ContaminaCacheYFallaAlLeer() {
        // Arrange
        await Cache.AddAsync(null!);

        // Act
        var action = () => Cache.GetAsync(99);

        // Assert
        await action.Should().ThrowAsync<NullReferenceException>();
    }
}
