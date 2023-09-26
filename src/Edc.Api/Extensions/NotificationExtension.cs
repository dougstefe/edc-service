using Edc.Core.Notifications;

namespace Edc.Api.Extensions;

public static class NotificationExtension {
    public static IResult GetResult(this Notification notification) {
        var statusCode = notification.NotificationMessages.Select(x => x.Type).OrderDescending().FirstOrDefault();

        var messages = notification.NotificationMessages.Select(x => x.Message).Distinct();

        return Results.Json(data: messages, statusCode: (int) statusCode);
    }
}
