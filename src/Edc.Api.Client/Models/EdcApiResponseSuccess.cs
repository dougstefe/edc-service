namespace Edc.Api.Client.Models;

public class EdcApiResponseSuccess<T> : EdcApiResponse {
    public T Data { get; set; }
}