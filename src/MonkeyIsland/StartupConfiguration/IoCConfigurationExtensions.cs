using MonkeyIsland.Core;

namespace MonkeyIsland.StartupConfiguration;

internal static class IoCConfigurationExtensions
{
    public static IServiceCollection ConfigureIoC(this IServiceCollection services)
    {
        services.AddSingleton<MagicNumberHandler>();

        return services;
    }
}
