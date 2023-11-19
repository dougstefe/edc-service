using Edc.Api.Client.Definitions;
using Edc.Api.Client.Extensions;
using Edc.Api.Client.Models;

namespace Edc.Api.Client.Implementations;

public class EdcApiClient : IEdcApiClient
{
    private readonly HttpClient _httpClient;

    public EdcApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<EdcApiResponse> GetProductsAsync()
    {
        var response = await _httpClient.GetAsync($"/api/v1/products");
        return await response.GetResponse<IEnumerable<Product>>();
    }
}