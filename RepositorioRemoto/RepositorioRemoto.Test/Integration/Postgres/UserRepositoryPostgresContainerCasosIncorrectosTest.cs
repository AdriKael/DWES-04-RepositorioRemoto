using FluentAssertions;
using RepositorioRemoto.Repositories;

namespace RepositorioRemoto.Test.Integration;

/// <summary>
/// Casos incorrectos / NotFound de <see cref="UserRepository"/> con PostgreSQL real.
/// </summary>
[TestFixture]
public class UserRepositoryPostgresContainerCasosIncorrectosTest : UserRepositoryPostgresContainerTestBase
{
    [Test]
    public async Task GetById_NonExistingUser_ReturnsFailure()
    {
        // Arrange
        var idInexistente = 9999;

        // Act
        var resultado = await Repository.GetByIdAsync(idInexistente);

        // Assert
        resultado.IsFailure.Should().BeTrue();
    }

    [Test]
    public async Task Update_NonExistingUser_ReturnsFailure()
    {
        // Arrange
        var user = CreateUser(9999);

        // Act
        var resultado = await Repository.UpdateAsync(9999, user);

        // Assert
        resultado.IsFailure.Should().BeTrue();
    }

    [Test]
    public async Task Delete_NonExistingUser_ReturnsFailure()
    {
        // Act
        var resultado = await Repository.DeleteAsync(9999);

        // Assert
        resultado.IsFailure.Should().BeTrue();
    }
}
