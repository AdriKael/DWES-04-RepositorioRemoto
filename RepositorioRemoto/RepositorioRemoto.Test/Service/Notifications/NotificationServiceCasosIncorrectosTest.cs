using FluentAssertions;
using RepositorioRemoto.Service.Notifications;

namespace RepositorioRemoto.Test.Service.Notifications;

[TestFixture]
public class NotificationServiceCasosIncorrectosTest {
    private NotificationService _service = null!;

    [SetUp]
    public void SetUp() {
        _service = new NotificationService();
    }

    [TearDown]
    public void TearDown() {
        _service.Dispose();
    }

    [Test]
    public void Notificar_NotificacionNula_LanzaExcepcion() {
        // Arrange

        // Act
        var action = () => _service.Notificar(null!);

        // Assert
        action.Should().Throw<NullReferenceException>();
    }
}
