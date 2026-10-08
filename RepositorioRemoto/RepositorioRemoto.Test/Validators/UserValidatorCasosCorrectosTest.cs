using FluentAssertions;
using RepositorioRemoto.Validators;

namespace RepositorioRemoto.Test.Validators;

[TestFixture]
public class UserValidatorCasosCorrectosTest {
    private UserValidator _validator = null!;

    [SetUp]
    public void SetUp() {
        _validator = new UserValidator();
    }

    [Test]
    public void Validate_UsuarioValido_RetornaExito() {
        // Arrange
        var user = UserTestData.CreateValidUser();

        // Act
        var result = _validator.Validate(user);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(user);
    }
}
