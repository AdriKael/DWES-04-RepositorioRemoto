using System.Text.Json;
using FluentAssertions;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Test.Storages;

[TestFixture]
public class UserStorageCasosCorrectosTest : UserStorageTestBase {
    [Test]
    public async Task ExportarJsonAsync_RutaValida_CreaArchivoConUsuarios() {
        // Arrange
        var path = Path.Combine(TestDirectory, "users.json");
        var users = new[] { CreateUser(1) };

        // Act
        var result = await Storage.ExportarJsonAsync(users, path);

        // Assert
        result.IsSuccess.Should().BeTrue();
        File.Exists(path).Should().BeTrue();
        var json = await File.ReadAllTextAsync(path);
        JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        }).Should().ContainSingle(user => user.Id == 1);
    }

    [Test]
    public async Task ExportarJsonAsync_DirectorioNoExistente_CreaDirectorioYArchivo() {
        // Arrange
        var path = Path.Combine(TestDirectory, "nested", "users.json");

        // Act
        var result = await Storage.ExportarJsonAsync([CreateUser(1)], path);

        // Assert
        result.IsSuccess.Should().BeTrue();
        Directory.Exists(Path.GetDirectoryName(path)!).Should().BeTrue();
        File.Exists(path).Should().BeTrue();
    }

    [Test]
    public async Task ExportarJsonAsync_ColeccionVacia_CreaJsonValido() {
        // Arrange
        var path = Path.Combine(TestDirectory, "empty.json");

        // Act
        var result = await Storage.ExportarJsonAsync([], path);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await File.ReadAllTextAsync(path)).Should().Contain("[");
    }
}
