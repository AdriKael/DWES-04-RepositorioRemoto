using FluentAssertions;
using RepositorioRemoto.Repositories;

namespace RepositorioRemoto.Test.Integration;

/// <summary>
/// Casos correctos de <see cref="UserRepository"/> con PostgreSQL real.
/// </summary>
[TestFixture]
public class UserRepositoryPostgresContainerCasosCorrectosTest : UserRepositoryPostgresContainerTestBase
{
    [Test]
    public async Task Create_InsertsUser()
    {
        // Arrange
        var user = CreateUser(1);

        // Act
        var resultado = await Repository.CreateAsync(user);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Id.Should().Be(1);
        resultado.Value.Name.Should().Be("Name1");
    }

    [Test]
    public async Task GetById_ExistingUser_ReturnsUser()
    {
        // Arrange
        var creado = CreateUser(1);
        await Repository.CreateAsync(creado);

        // Act
        var resultado = await Repository.GetByIdAsync(1);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Should().BeEquivalentTo(creado);
    }

    [Test]
    public async Task GetAll_ReturnsAllUsersOrderedById()
    {
        // Arrange
        await Repository.CreateAsync(CreateUser(2));
        await Repository.CreateAsync(CreateUser(1));
        await Repository.CreateAsync(CreateUser(3));

        // Act
        var resultados = await Repository.GetAllAsync();

        // Assert
        resultados.Select(u => u.Id).Should().Equal(1, 2, 3);
    }

    [Test]
    public async Task Update_ExistingUserWithDetachedInstance_ReturnsUpdated()
    {
        // Arrange: instancia detached con datos modificados (caso real DTO).
        // Requiere el fix de UpdateAsync con AsNoTracking en la comprobación
        // de existencia; sin él falla con "already being tracked".
        await Repository.CreateAsync(CreateUser(1));
        Context.ChangeTracker.Clear();
        var actualizado = CreateUser(1) with { Name = "Actualizado" };

        // Act
        var resultado = await Repository.UpdateAsync(1, actualizado);

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Name.Should().Be("Actualizado");

        var verificacion = await Repository.GetByIdAsync(1);
        verificacion.Value.Name.Should().Be("Actualizado");
    }

    [Test]
    public async Task Delete_ExistingUser_RemovesUser()
    {
        // Arrange
        await Repository.CreateAsync(CreateUser(1));

        // Act
        var eliminado = await Repository.DeleteAsync(1);

        // Assert
        eliminado.IsSuccess.Should().BeTrue();

        var verificacion = await Repository.GetByIdAsync(1);
        verificacion.IsFailure.Should().BeTrue();
    }

    [Test]
    public async Task DeleteAll_WithUsers_RemovesAll()
    {
        // Arrange
        await Repository.CreateAsync(CreateUser(1));
        await Repository.CreateAsync(CreateUser(2));

        // Act
        var resultado = await Repository.DeleteAllAsync();

        // Assert
        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Should().BeTrue();
        (await Repository.GetAllAsync()).Should().BeEmpty();
    }
}
