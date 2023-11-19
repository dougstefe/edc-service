using System.Text.Json;
using Edc.Api.Client.Models;

namespace Edc.Api.Client.Extensions;

public static class HttpResponseExtensions {
    public static async Task<EdcApiResponse> GetResponse<T>(this HttpResponseMessage httpResponseMessage){
        var message = await httpResponseMessage.Content.ReadAsStringAsync();
        if (httpResponseMessage.IsSuccessStatusCode) {
            return new EdcApiResponseSuccess<T> {
                StatusCode = httpResponseMessage.StatusCode,
                Data = JsonSerializer.Deserialize<T>(message)
            };
        }
        else {
            return new EdcApiResponseError {
                StatusCode = httpResponseMessage.StatusCode,
                ErrorMessage = message
            };
        }
    }
}