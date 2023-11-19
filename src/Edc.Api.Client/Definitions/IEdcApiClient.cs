using Edc.Api.Client.Models;

namespace Edc.Api.Client.Definitions;

public interface IEdcApiClient {
    Task<EdcApiResponse> GetProductsAsync();
}