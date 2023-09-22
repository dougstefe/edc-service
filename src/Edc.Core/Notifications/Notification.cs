namespace Edc.Core.Notifications;

public class Notification {
    private List<NotificationMessage> _notificationMessages { get; set; } = new();

    public IReadOnlyCollection<NotificationMessage> NotificationMessages => _notificationMessages;
    public bool HasMessages => NotificationMessages.Any();
    

    public void AddNotificationMessage(string message, NotificationMessageType type = NotificationMessageType.DomainNotification)
    {
        _notificationMessages.Add(new NotificationMessage(message, type));
    }
}