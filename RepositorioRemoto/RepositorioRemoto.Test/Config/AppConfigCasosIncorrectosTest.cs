using FluentAssertions;
using RepositorioRemoto.Config;

namespace RepositorioRemoto.Test;

[TestFixture]
public class AppConfigCasosIncorrectosTest {
    [Test]
    public void Configure_PerfilInexistente_LanzaFileNotFoundException() {
        // Arrange

        // Act
        var action = () => AppConfig.Configure("perfil-inexistente");

        // Assert
        action.Should().Throw<FileNotFoundException>();
    }
}
