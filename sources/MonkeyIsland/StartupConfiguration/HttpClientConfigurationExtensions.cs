using MonkeyIsland.Core;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace MonkeyIsland.StartupConfiguration;

internal static class HttpClientConfigurationExtensions
{
    public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<SecretApiHttpClient>();
        return services;
    }

    private static IHttpClientBuilder AddHttpClient<T>(this IServiceCollection services, int maxRetryCount = 3, TimeSpan? medianFirstRetryDelay = null, bool allowInvalidCertificate = false) where T : class
    {
        medianFirstRetryDelay ??= TimeSpan.FromMilliseconds(250);

        var httpBuilder = services.AddHttpClient<T>(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        })
            .AddPolicyHandler(GetRetryPolicy<T>(maxRetryCount, medianFirstRetryDelay.Value));

        if (allowInvalidCertificate)
        {
            httpBuilder.ConfigurePrimaryHttpMessageHandler(ServiceProvider => new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
        }
        return httpBuilder;
    }
    private static Func<IServiceProvider, HttpRequestMessage, IAsyncPolicy<HttpResponseMessage>> GetRetryPolicy<T>(int maxRetryCount, TimeSpan medianFirstRetryDelay)
    {
        return (provider, message) =>
        {
            IEnumerable<TimeSpan>? delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay, maxRetryCount);
            ILogger? logger = provider.GetService<ILogger<T>>();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(delay, (result, timeSpan, retryCount, context) =>
                {
                    if (result.Result?.StatusCode == null)
                    {
                        logger?.LogWarning("Request failed because of network failure. Waiting {TimeSpan} before next retry. Retry attempt {RetryCount}", timeSpan, retryCount);
                    }
                    else
                    {
                        logger?.LogWarning("Request failed with {StatusCode}. Waiting {TimeSpan} before next retry. Retry attempt {RetryCount}", result.Result.StatusCode, timeSpan, retryCount);
                    }
                });
        };
    }
}
