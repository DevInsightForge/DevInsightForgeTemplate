using DevInsightForge.Application.Behaviours;
using DevInsightForge.Application.Configurations.Mapster;
using DevInsightForge.Application.Configurations.Settings;
using DevInsightForge.Domain.Entities.User;
using FluentValidation.AspNetCore;
using FluentValidation;
using Mapster;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevInsightForge.Application.Services;

namespace DevInsightForge.Application;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Inject and Configure Mediatr
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServices).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });

        // Bind Settings from configurations
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));


        // Inject and Configure Fluent Validation
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(typeof(ApplicationServices).Assembly);
        services.AddFluentValidationRulesToSwagger();

        // Configure Mapster
        TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);
        MappingConfigurations.ConfigureMappings();

        // Inject password hasher
        services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
        services.AddScoped<TokenServices>();

        return services;
    }
}
