namespace Edc.Core.Notifications;

public class NotificationMessage {
    public NotificationMessage(string message, NotificationMessageType type)
    {
        Message = message;
        Type = type;
    }

    public string Message { get; set; } = String.Empty;
    public NotificationMessageType Type { get; set; } = NotificationMessageType.DomainNotification;
}

public enum NotificationMessageType {
    DomainNotification = 400,
    AuthorizationNotification = 401,
    NotFoundNotification = 404,
    ServerErrorNotification = 500
}