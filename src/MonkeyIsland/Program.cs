using MonkeyIsland;
using NLog;
using NLog.Web;
using Steeltoe.Extensions.Configuration.Placeholder;


var host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddEnvironmentVariables("GF_");
        })
        .AddPlaceholderResolver()
        .ConfigureLogging((context, builder) =>
        {
            LogManager.Setup().LoadConfigurationFromFile();
            builder.ClearProviders();
        })
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
        .UseContentRoot(AppContext.BaseDirectory)
        .UseNLog()
        .Build();

host.Run();
