﻿using DevInsightForge.Application.Interfaces.DataAccess;
using DevInsightForge.Application.Interfaces.DataAccess.Repositories;
using DevInsightForge.Infrastructure.DataAccess;
using DevInsightForge.Infrastructure.DataAccess.Repositories;
using DevInsightForge.Infrastructure.Persistence;
using DevInsightForge.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevInsightForge.Infrastructure;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure DbContext provider
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("SqliteDatabase"));
            options.ReplaceService<IValueConverterSelector, CustomValueConverterSelector>();
        });

        // Register data-access services
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
