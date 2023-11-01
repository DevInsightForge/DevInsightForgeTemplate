using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace DevInsightForge.WebAPI.Extensions;

public static class SwaggerServiceExtension
{
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        // Inject Swagger/OpenAPI Service
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(setup =>
        {
            // Application Specifications
            setup.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Career Web API",
                Version = "v1",
                Description = "The Career Web API built with ASP.NET Core, it ensures secure and efficient communication through JSON Web Tokens (JWT) for authentication.",
                Contact = new OpenApiContact
                {
                    Name = "ZèD",
                    Email = "imzihad@gmail.com",
                    Url = new Uri("https://imzihad21.github.io")
                }
            });
            // Include 'SecurityScheme' to use JWT Authentication
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on the textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
        });

        return services;
    }
}
