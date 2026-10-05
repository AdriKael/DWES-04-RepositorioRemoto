using RepositorioRemoto.Enums;

namespace RepositorioRemoto.Models;

public record Notification(TypeNotification Type, string Message, DateTime Date);