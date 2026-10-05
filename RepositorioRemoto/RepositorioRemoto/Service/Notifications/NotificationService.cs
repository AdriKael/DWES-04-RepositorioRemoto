using System.Reactive.Linq;
using System.Reactive.Subjects;
using RepositorioRemoto.Models;
using Serilog;

namespace RepositorioRemoto.Service.Notifications;

public class NotificationService : INotificationService, IDisposable {
    private static readonly ILogger _logger = Log.ForContext<NotificationService>();
    private readonly Subject<Notification> _subject = new();

    public void Dispose() {
        _logger.Debug("[NOTIF-DISPOSE] Cerrando servicio de notificaciones");
        _subject.OnCompleted();
        _subject.Dispose();
    }

    public IObservable<Notification> Observable {
        get {
            _logger.Debug("[NOTIF-OBSERVABLE] Suscripción al servicio de notificaciones");
            return _subject.AsObservable();
        }
    }

    public void Notificar(Notification notification) {
        _logger.Debug("[NOTIF-SEND] {Type}: {Message}", notification.Type, notification.Message);
        _subject.OnNext(notification);
    }
}