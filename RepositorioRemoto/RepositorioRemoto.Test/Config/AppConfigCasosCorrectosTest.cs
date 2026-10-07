using FluentAssertions;
using RepositorioRemoto.Config;

namespace RepositorioRemoto.Test.Config;

[TestFixture]
public class AppConfigCasosCorrectosTest {
    [SetUp]
    public void SetUp() {
        AppConfig.Configure("dev");
    }

    [Test]
    public void Configure_PerfilDev_CargaValoresDeConfiguracion() {
        // Arrange

        // Act
        var profile = AppConfig.Profile;
        var apiUrl = AppConfig.ApiBaseUrl;
        var cacheMinutes = AppConfig.CacheExpirationMinutes;
        var syncSeconds = AppConfig.SyncIntervalSeconds;

        // Assert
        profile.Should().Be("dev");
        apiUrl.Should().Be("https://jsonplaceholder.typicode.com/");
        cacheMinutes.Should().Be(10);
        syncSeconds.Should().Be(60);
    }

    [Test]
    public void UsersJsonPath_ConfiguracionCargada_UsaDataFolder() {
        // Arrange
        AppConfig.Configure("dev");

        // Act
        var path = AppConfig.UsersJsonPath;

        // Assert
        Path.GetFileName(path).Should().Be("users.json");
        Path.GetFileName(Path.GetDirectoryName(path)!).Should().Be("data");
    }

    [Test]
    public void SqliteConnection_ConfiguracionCargada_CreaRutaDeBaseDeDatos() {
        // Arrange
        AppConfig.Configure("dev");

        // Act
        var connection = AppConfig.SqliteConnection;

        // Assert
        connection.Should().Contain("Data Source=");
        connection.Should().Contain("users.db");
    }
}
