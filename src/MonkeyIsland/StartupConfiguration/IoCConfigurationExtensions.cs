using MonkeyIsland.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyIsland.StartupConfiguration;

internal static class IoCConfigurationExtensions
{
    public static IServiceCollection ConfigureIoC(this IServiceCollection services)
    {
        services.AddSingleton<MagicNumberHandler>();

        return services;
    }
}
