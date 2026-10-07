using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using RepositorioRemoto.Errors;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Test.Service.Users;

[TestFixture]
public class UserServiceCasosCorrectosTest : UserServiceTestBase {
    [Test]
    public async Task GetAllAsync_ConUsuariosLocales_RetornaLosLocalesSinConsultarApi() {
        // Arrange
        var users = new List<User> { CreateUser(1), CreateUser(2) };
        Repository.Setup(x => x.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await Service.GetAllAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(users);
        Api.Verify(x => x.GetUsuariosAsync(), Times.Never);
    }

    [Test]
    public async Task GetAllAsync_SinUsuariosLocales_CargaYGuardaLosRemotos() {
        // Arrange
        var remote = new List<User> { CreateUser(1), CreateUser(2) };
        Repository.Setup(x => x.GetAllAsync()).ReturnsAsync([]);
        Api.Setup(x => x.GetUsuariosAsync()).ReturnsAsync(remote);

        // Act
        var result = await Service.GetAllAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        Repository.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Exactly(2));
    }

    [Test]
    public async Task GetByIdAsync_EnCache_RetornaCacheSinConsultarRepositorio() {
        // Arrange
        var user = CreateUser(1);
        Cache.Setup(x => x.GetAsync(1)).ReturnsAsync(user);

        // Act
        var result = await Service.GetByIdAsync(1);

        // Assert
        result.Value.Should().Be(user);
        Repository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Test]
    public async Task GetByIdAsync_EnRepositorio_AñadeCacheYRetornaUsuario() {
        // Arrange
        var user = CreateUser(1);
        Cache.Setup(x => x.GetAsync(1)).ReturnsAsync((User?)null);
        Repository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Result.Success<User, DomainErrors>(user));

        // Act
        var result = await Service.GetByIdAsync(1);

        // Assert
        result.Value.Should().Be(user);
        Cache.Verify(x => x.AddAsync(user), Times.Once);
    }

    [Test]
    public async Task GetByIdAsync_EnApi_CreaAñadeCacheYRetornaUsuario() {
        // Arrange
        var user = CreateUser(1);
        Cache.Setup(x => x.GetAsync(1)).ReturnsAsync((User?)null);
        Repository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Result.Failure<User, DomainErrors>(UsersError.NotFoundError(1)));
        Api.Setup(x => x.GetUsuarioByIdAsync(1)).ReturnsAsync(user);
        Repository.Setup(x => x.CreateAsync(user)).ReturnsAsync(Result.Success<User, DomainErrors>(user));

        // Act
        var result = await Service.GetByIdAsync(1);

        // Assert
        result.Value.Should().Be(user);
        Repository.Verify(x => x.CreateAsync(user), Times.Once);
        Cache.Verify(x => x.AddAsync(user), Times.Once);
    }

    [Test]
    public async Task CreateAsync_DatosValidos_CreaGuardaYNotifica() {
        // Arrange
        var request = CreateRequest();
        var created = CreateUser(9);
        var saved = CreateUser(9);
        Validator.Setup(x => x.Validate(It.IsAny<User>())).Returns((User user) => Result.Success<User, DomainErrors>(user));
        Api.Setup(x => x.CreateUsuarioAsync(request)).ReturnsAsync(created);
        Repository.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(Result.Success<User, DomainErrors>(saved));

        // Act
        var result = await Service.CreateAsync(request);

        // Assert
        result.Value.Should().Be(saved);
        Notifications.Verify(x => x.Notificar(It.IsAny<Notification>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_UsuarioExistente_ActualizaInvalidaCacheYNotifica() {
        // Arrange
        var request = CreateUpdateRequest(1);
        Repository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Result.Success<User, DomainErrors>(CreateUser(1)));
        Api.Setup(x => x.UpdateUsuarioAsync(1, request)).ReturnsAsync(CreateUser(1));
        Repository.Setup(x => x.UpdateAsync(1, It.IsAny<User>())).ReturnsAsync(Result.Success<User, DomainErrors>(CreateUser(1)));

        // Act
        var result = await Service.UpdateAsync(1, request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        Cache.Verify(x => x.RemoveAsync(1), Times.Once);
        Notifications.Verify(x => x.Notificar(It.IsAny<Notification>()), Times.Once);
    }

    [Test]
    public async Task ExportToJsonAsync_ExportacionCorrecta_RetornaTrue() {
        // Arrange
        var users = new List<User> { CreateUser(1) };
        Repository.Setup(x => x.GetAllAsync()).ReturnsAsync(users);
        Storage.Setup(x => x.ExportarJsonAsync(users, It.IsAny<string>())).ReturnsAsync(Result.Success<bool, DomainErrors>(true));

        // Act
        var result = await Service.ExportToJsonAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeTrue();
    }

    [Test]
    public async Task DeleteAsync_UsuarioExistente_EliminaInvalidaCacheYNotifica() {
        // Arrange
        Repository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(Result.Success<User, DomainErrors>(CreateUser(1)));
        Api.Setup(x => x.DeleteUsuarioAsync(1)).Returns(Task.CompletedTask);
        Repository.Setup(x => x.DeleteAsync(1)).ReturnsAsync(Result.Success<User, DomainErrors>(CreateUser(1)));

        // Act
        var result = await Service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        Cache.Verify(x => x.RemoveAsync(1), Times.Once);
        Notifications.Verify(x => x.Notificar(It.IsAny<Notification>()), Times.Once);
    }
}
