﻿using DevInsightForge.Application.Common.Behaviours;
using DevInsightForge.Application.Common.Configurations.Mapster;
using DevInsightForge.Application.Common.Configurations.Settings;
using DevInsightForge.Application.Common.Interfaces;
using DevInsightForge.Application.Common.Interfaces.Core;
using DevInsightForge.Application.Common.Services;
using DevInsightForge.Domain.Entities.Core;
using DevInsightForge.WebAPI.Services;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        // Bind context user as authenticated user
        services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();

        // Inject password hasher
        services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();

        // Register application services
        services.AddScoped<ITokenService, TokenServices>();

        return services;
    }
}
