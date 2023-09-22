using Edc.Core.Notifications;

namespace Edc.Api.Extensions;

public static class NotificationExtension {
    public static IResult GetResult(this Notification notification) {
        var statusCode =
            notification.NotificationMessages.Any(x => x.Type == NotificationMessageType.ServerErrorNotification) ?
            StatusCodes.Status500InternalServerError :
            StatusCodes.Status400BadRequest;

        var messages = notification.NotificationMessages.Select(x => x.Message).Distinct();

        return Results.Json(data: messages, statusCode: statusCode);
    }
}
