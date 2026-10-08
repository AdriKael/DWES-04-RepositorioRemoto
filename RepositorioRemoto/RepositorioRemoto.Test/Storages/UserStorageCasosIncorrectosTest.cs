using FluentAssertions;
using RepositorioRemoto.Errors;

namespace RepositorioRemoto.Test.Storages;

[TestFixture]
public class UserStorageCasosIncorrectosTest : UserStorageTestBase {
    [Test]
    public async Task ExportarJsonAsync_RutaEsDirectorio_RetornaErrorDeEscritura() {
        // Arrange
        Directory.CreateDirectory(TestDirectory);

        // Act
        var result = await Storage.ExportarJsonAsync([CreateUser(1)], TestDirectory);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<StorageErrors.WriteError>();
    }

    [Test]
    public async Task ExportarJsonAsync_ColeccionNula_RetornaErrorDeEscritura() {
        // Arrange
        var path = Path.Combine(TestDirectory, "users.json");

        // Act
        var result = await Storage.ExportarJsonAsync(null!, path);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<StorageErrors.WriteError>();
    }
}
