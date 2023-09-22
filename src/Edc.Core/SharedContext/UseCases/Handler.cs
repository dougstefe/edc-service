using Edc.Core.Exceptions;
using Edc.Core.Notifications;

namespace Edc.Core.SharedContext.UseCases;

public abstract class Handler {
    protected (NotificationMessageType type, string error) GenerateResponseError(Exception ex) {
        NotificationMessageType type;
        switch (ex) {
            case ArgumentException:
            case DomainException:
                type = NotificationMessageType.DomainNotification;
                break;

            default:
                type = NotificationMessageType.ServerErrorNotification;
                break;
        }
        return (type, ex.Message);
    }
}