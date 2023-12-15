﻿using DevInsightForge.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;

namespace DevInsightForge.Infrastructure.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    #region Entity DB Set Registrations
    public DbSet<UserModel> Users { get; set; }

    #endregion


    #region EF Core Configuration Overrides
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    #endregion
}
