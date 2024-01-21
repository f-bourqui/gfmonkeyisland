using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MonkeyIsland.StartupConfiguration
{
    internal static class SwaggerConfigurationExtensions
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var openApiInfo = configuration.GetSection(nameof(OpenApiInfo)).Get<OpenApiInfo>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", openApiInfo);
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }
    }
}
