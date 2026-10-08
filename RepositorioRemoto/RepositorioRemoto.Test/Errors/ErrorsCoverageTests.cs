using FluentAssertions;
using RepositorioRemoto.Errors;

namespace RepositorioRemoto.Test.Errors;

/// <summary>
/// Tests de cobertura para los errores de API y Servicio.
/// Son records/factorías simples sin infraestructura externa.
/// </summary>
[TestFixture]
public class ErrorsCoverageTests
{
    [Test]
    public void ApiErrors_StoresMessage()
    {
        // Arrange
        const string message = "fallo api";

        // Act
        var error = new ApiErrors(message);

        // Assert
        error.Message.Should().Be(message);
    }

    [Test]
    public void ServiceError_NotFoundError_ContainsId()
    {
        // Act
        var error = ServiceError.NotFoundError(7);

        // Assert
        error.Should().BeOfType<ServiceErrors.NotFoundError>();
        error.Message.Should().Contain("7");
    }

    [Test]
    public void ServiceError_GetAllError_ReturnsInstance()
    {
        // Act
        var error = ServiceError.GetAllError();

        // Assert
        error.Should().BeOfType<ServiceErrors.GetAllError>();
    }

    [Test]
    public void ServiceError_GetByIdError_ContainsId()
    {
        // Act
        var error = ServiceError.GetByIdError(3);

        // Assert
        error.Should().BeOfType<ServiceErrors.GetByIdError>();
        error.Message.Should().Contain("3");
    }

    [Test]
    public void ServiceError_CreateError_ReturnsInstance()
    {
        // Act
        var error = ServiceError.CreateError();

        // Assert
        error.Should().BeOfType<ServiceErrors.CreateError>();
    }

    [Test]
    public void ServiceError_UpdateError_ContainsId()
    {
        // Act
        var error = ServiceError.UpdateError(5);

        // Assert
        error.Should().BeOfType<ServiceErrors.UpdateError>();
        error.Message.Should().Contain("5");
    }

    [Test]
    public void ServiceError_DeleteError_ContainsId()
    {
        // Act
        var error = ServiceError.DeleteError(9);

        // Assert
        error.Should().BeOfType<ServiceErrors.DeleteError>();
        error.Message.Should().Contain("9");
    }

    [Test]
    public void ServiceError_ExportToJsonError_ContainsDetail()
    {
        // Act
        var error = ServiceError.ExportToJsonError("detalle");

        // Assert
        error.Should().BeOfType<ServiceErrors.ExportToJsonError>();
        error.Message.Should().Contain("detalle");
    }
}
