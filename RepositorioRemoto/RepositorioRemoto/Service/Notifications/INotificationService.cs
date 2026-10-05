using RepositorioRemoto.Models;

namespace RepositorioRemoto.Service.Notifications;

public interface INotificationService {
    IObservable<Notification> Observable { get; }

    void Notificar(Notification notification);
}