using MonkeyIsland.HostedServices;

namespace MonkeyIsland.StartupConfiguration;

internal static class HostedServiceConfigurationExtensions
{
    public static IServiceCollection ConfigureHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<SecretApiWorker>();
        return services;
    }
}
