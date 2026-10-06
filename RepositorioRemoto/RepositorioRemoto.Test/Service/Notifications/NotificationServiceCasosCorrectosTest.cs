using FluentAssertions;
using RepositorioRemoto.Enums;
using RepositorioRemoto.Models;
using RepositorioRemoto.Service.Notifications;

namespace RepositorioRemoto.Test;

[TestFixture]
public class NotificationServiceCasosCorrectosTest {
    private NotificationService _service = null!;
    private bool _disposed;

    [SetUp]
    public void SetUp() {
        _service = new NotificationService();
    }

    [TearDown]
    public void TearDown() {
        if (!_disposed) _service.Dispose();
    }

    [Test]
    public void Notificar_NotificacionValida_PublicaEnObservable() {
        // Arrange
        var received = new List<Notification>();
        var notification = CreateNotification(TypeNotification.Create);
        using var subscription = _service.Observable.Subscribe(received.Add);

        // Act
        _service.Notificar(notification);

        // Assert
        received.Should().ContainSingle().Which.Should().Be(notification);
    }

    [Test]
    public void Notificar_VariasNotificaciones_MantieneElOrden() {
        // Arrange
        var received = new List<Notification>();
        using var subscription = _service.Observable.Subscribe(received.Add);
        var notifications = new[] {
            CreateNotification(TypeNotification.Create),
            CreateNotification(TypeNotification.Update),
            CreateNotification(TypeNotification.Delete)
        };

        // Act
        foreach (var notification in notifications) _service.Notificar(notification);

        // Assert
        received.Should().Equal(notifications);
    }

    [Test]
    public void Dispose_ServicioActivo_CompletaObservable() {
        // Arrange
        var completed = false;
        _service.Observable.Subscribe(_ => { }, () => completed = true);

        // Act
        _service.Dispose();
        _disposed = true;

        // Assert
        completed.Should().BeTrue();
    }

    private static Notification CreateNotification(TypeNotification type) =>
        new(type, "Test notification", DateTime.UtcNow);
}
