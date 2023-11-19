using System.Net;
using System.Net.Http.Headers;
using Edc.Api.Client.Definitions;
using Edc.Api.Client.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Edc.Api.Client.Extensions;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddEdcAPIClient(this IServiceCollection services, Func<IServiceProvider, Settings> clientConfig)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddHttpClient<IEdcApiClient, EdcApiClient>((s, c) =>
            {
                var config = clientConfig.Invoke(s);
                c.BaseAddress = new Uri(config.BaseUri);
                c.DefaultRequestHeaders.Add("access-token", config.AccessToken);
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (config.Timeout.HasValue)
                {
                    c.Timeout = config.Timeout.Value;
                }
            }).AddPolicyHandler((s, _) =>
            {
                var config = clientConfig.Invoke(s);
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(
                        response => response.StatusCode.Equals(HttpStatusCode.InternalServerError) || response.StatusCode.Equals(HttpStatusCode.BadGateway) || response.StatusCode.Equals(HttpStatusCode.ServiceUnavailable) || response.StatusCode.Equals(HttpStatusCode.GatewayTimeout)
                    ).WaitAndRetryAsync(
                        config.MaximumRetryAttempts,
                        retry => TimeSpan.FromSeconds(Math.Pow(2, retry))
                    );
            }).AddPolicyHandler((s, _) =>
            {
                var config = clientConfig.Invoke(s);
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                       handledEventsAllowedBeforeBreaking: config.MaximumConsecutiveFailures,
                       durationOfBreak: TimeSpan.FromSeconds(config.MaximumCircuitBreakerWaitingTime)
                    );
            });
            return services;
        }
}
