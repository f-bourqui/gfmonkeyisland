using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using MonkeyIsland.StartupConfiguration;

namespace MonkeyIsland;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

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
