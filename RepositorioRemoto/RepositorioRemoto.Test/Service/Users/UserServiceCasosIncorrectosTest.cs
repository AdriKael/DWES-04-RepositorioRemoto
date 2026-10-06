using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using RepositorioRemoto.Errors;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Test;

[TestFixture]
public class UserServiceCasosIncorrectosTest : UserServiceTestBase {
    [Test]
    public async Task GetByIdAsync_NoEncontradoEnNingunaFuente_RetornaNotFound() {
        // Arrange
        Cache.Setup(x => x.GetAsync(1)).ReturnsAsync((User?)null);
        Repository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Result.Failure<User, DomainErrors>(UsersError.NotFoundError(1)));
        Api.Setup(x => x.GetUsuarioByIdAsync(1)).ReturnsAsync((User?)null);

        // Act
        var result = await Service.GetByIdAsync(1);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UserErrors.NotFoundError>();
    }

    [Test]
    public async Task CreateAsync_NombreVacio_NoConsultaApiYRetornaValidacion() {
        // Arrange
        var request = CreateRequest() with { Name = "" };
        Validator.Setup(x => x.Validate(It.IsAny<User>())).Returns(Result.Failure<User, DomainErrors>(UsersError.ValidationError("Name", "obligatorio")));

        // Act
        var result = await Service.CreateAsync(request);

        // Assert
        result.Error.Should().BeOfType<UserErrors.ValidationError>();
        Api.Verify(x => x.CreateUsuarioAsync(It.IsAny<RepositorioRemoto.Dto.CreateUserRequest>()), Times.Never);
    }

    [Test]
    public async Task DeleteAsync_UsuarioInexistente_NoConsultaApiYRetornaNotFound() {
        // Arrange
        Repository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Result.Failure<User, DomainErrors>(UsersError.NotFoundError(1)));

        // Act
        var result = await Service.DeleteAsync(1);

        // Assert
        result.Error.Should().BeOfType<UserErrors.NotFoundError>();
        Api.Verify(x => x.DeleteUsuarioAsync(It.IsAny<int>()), Times.Never);
    }
}
