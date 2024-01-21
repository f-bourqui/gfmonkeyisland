using MonkeyIsland.Core.Settings;

namespace MonkeyIsland.StartupConfiguration;

internal static class SettingsConfigurationExtensions
{
    public static IServiceCollection ConfigureSettings(this IServiceCollection services)
    {
        services.AddOptions<ApiSettings>().BindConfiguration("SecretAPI").ValidateDataAnnotations().ValidateOnStart();
        return services;
    }
}
