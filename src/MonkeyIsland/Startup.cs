using MonkeyIsland.StartupConfiguration;

namespace MonkeyIsland;
/// <summary>
/// Application startup code
/// </summary>
/// <param name="configuration">App configuration</param>

public class Startup(IConfiguration configuration)
{
    /// <summary>
    /// App configuration
    /// </summary>
    public IConfiguration Configuration { get; } = configuration;

    /// <summary>
    /// Configure service
    /// </summary>
    /// <param name="services">service collection</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true)
            .ConfigureSettings()
            .ConfigureSwagger(Configuration)
            .ConfigureHttpClients(Configuration)
            .ConfigureHostedServices()
            .ConfigureIoC();

    }

    /// <summary>
    /// Configure app
    /// </summary>
    /// <param name="appBuilder">app builder</param>
    /// <param name="environment">web host environment</param>
    public void Configure(IApplicationBuilder appBuilder, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            appBuilder.UseSwagger();
            appBuilder.UseSwaggerUI();
            appBuilder.UseDeveloperExceptionPage();
        }

        appBuilder.UseHttpsRedirection();
        appBuilder.UseRouting();
        appBuilder.UseAuthorization();

        appBuilder.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }
}
