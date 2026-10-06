using FluentAssertions;

namespace RepositorioRemoto.Test;

[TestFixture]
public class UserCacheCasosCorrectosTest : UserCacheTestBase {
    [Test]
    public async Task AddAsync_UsuarioNuevo_PermiteRecuperarlo() {
        // Arrange
        var user = CreateUser(1);

        // Act
        await Cache.AddAsync(user);

        // Assert
        (await Cache.GetAsync(1)).Should().BeEquivalentTo(user);
    }

    [Test]
    public async Task AddAsync_MismoId_ReemplazaUsuarioAnterior() {
        // Arrange
        await Cache.AddAsync(CreateUser(1, "Old"));
        var updated = CreateUser(1, "Updated");

        // Act
        await Cache.AddAsync(updated);

        // Assert
        (await Cache.GetAsync(1)).Should().BeEquivalentTo(updated);
    }

    [Test]
    public async Task RemoveAsync_UsuarioExistente_NoPermiteRecuperarlo() {
        // Arrange
        await Cache.AddAsync(CreateUser(1));

        // Act
        await Cache.RemoveAsync(1);

        // Assert
        (await Cache.GetAsync(1)).Should().BeNull();
    }

    [Test]
    public async Task RemoveAsync_ConOtrosUsuarios_ConservaLosNoEliminados() {
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
    public async Task ClearAsync_ConUsuarios_LimpiaTodaLaCache() {
        // Arrange
        await Cache.AddAsync(CreateUser(1));

        // Act
        await Cache.ClearAsync();

        // Assert
        (await Cache.GetAsync(1)).Should().BeNull();
    }
}
