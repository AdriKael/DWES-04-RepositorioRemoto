using FluentAssertions;
using RepositorioRemoto.Errors;
using RepositorioRemoto.Models;
using RepositorioRemoto.Validators;

namespace RepositorioRemoto.Test;

[TestFixture]
public class UserValidatorCasosIncorrectosTest {
    private UserValidator _validator = null!;

    [SetUp]
    public void SetUp() {
        _validator = new UserValidator();
    }

    [Test]
    public void Validate_NombreVacio_RetornaErrorDeValidacion() {
        // Arrange
        var user = UserTestData.CreateValidUser() with { Name = "" };

        // Act
        var result = _validator.Validate(user);

        // Assert
        AssertValidationError(result, nameof(User.Name));
    }

    [Test]
    public void Validate_NombreDeUsuarioVacio_RetornaErrorDeValidacion() {
        // Arrange
        var user = UserTestData.CreateValidUser() with { UserName = " " };

        // Act
        var result = _validator.Validate(user);

        // Assert
        AssertValidationError(result, nameof(User.UserName));
    }

    [Test]
    public void Validate_EmailVacio_RetornaErrorDeValidacion() {
        // Arrange
        var user = UserTestData.CreateValidUser() with { Email = "" };

        // Act
        var result = _validator.Validate(user);

        // Assert
        AssertValidationError(result, nameof(User.Email));
    }

    [Test]
    public void Validate_CalleVacia_RetornaErrorDeValidacion() {
        // Arrange
        var user = UserTestData.CreateValidUser() with { Address = UserTestData.CreateValidUser().Address with { Street = "" } };

        // Act
        var result = _validator.Validate(user);

        // Assert
        AssertValidationError(result, nameof(Address.Street));
    }

    [Test]
    public void Validate_CiudadVacia_RetornaErrorDeValidacion() {
        // Arrange
        var user = UserTestData.CreateValidUser() with { Address = UserTestData.CreateValidUser().Address with { City = "" } };

        // Act
        var result = _validator.Validate(user);

        // Assert
        AssertValidationError(result, nameof(Address.City));
    }

    [Test]
    public void Validate_TelefonoVacio_RetornaErrorDeValidacion() {
        // Arrange
        var user = UserTestData.CreateValidUser() with { Phone = "" };

        // Act
        var result = _validator.Validate(user);

        // Assert
        AssertValidationError(result, nameof(User.Phone));
    }

    [Test]
    public void Validate_WebsiteVacio_RetornaErrorDeValidacion() {
        // Arrange
        var user = UserTestData.CreateValidUser() with { Website = "" };

        // Act
        var result = _validator.Validate(user);

        // Assert
        AssertValidationError(result, nameof(User.Website));
    }

    [Test]
    public void Validate_EmpresaSinNombre_RetornaErrorDeValidacion() {
        // Arrange
        var user = UserTestData.CreateValidUser() with { Company = UserTestData.CreateValidUser().Company with { Name = "" } };

        // Act
        var result = _validator.Validate(user);

        // Assert
        AssertValidationError(result, nameof(Company.Name));
    }

    private static void AssertValidationError(CSharpFunctionalExtensions.Result<User, DomainErrors> result, string field) {
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<UserErrors.ValidationError>();
        ((UserErrors.ValidationError)result.Error).Field.Should().Be(field);
    }
}

internal static class UserTestData {
    public static User CreateValidUser() => new(
        1,
        "Name",
        "user",
        "user@test.com",
        new Address("Street", "Suite", "City", "00000", new Geo("0", "0")),
        "555",
        "site",
        new Company("Company", "phrase", "bs"));
}
