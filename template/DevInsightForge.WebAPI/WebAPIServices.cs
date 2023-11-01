using DevInsightForge.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DevInsightForge.WebAPI;

public static class WebAPIServices
{
    public static IServiceCollection AddWebAPIServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Inject Controller Handlers
        services.AddControllers();

        // Disable inbuild model validators in favor of Fluent Validation
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        // Inject Extension services
        services.AddAuthenticationService(configuration);
        services.AddSwaggerService();
        services.AddCorsService();

        return services;
    }
}
