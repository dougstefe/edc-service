namespace Edc.Api.Client.Models;

public class EdcApiResponseError : EdcApiResponse {
    public string ErrorMessage { get; set; } = string.Empty;
}