using FluentAssertions;
using RepositorioRemoto.Config;
using RepositorioRemoto.Infrastructure;

namespace RepositorioRemoto.Test;

[TestFixture]
public class DependencyInjectionCasosIncorrectosTest {
    [Test]
    public void BuildServiceProvider_PerfilInexistente_NoPuedeConfigurarDependencias() {
        // Arrange

        // Act
        var action = () => AppConfig.Configure("perfil-inexistente");

        // Assert
        action.Should().Throw<FileNotFoundException>();
    }
}
