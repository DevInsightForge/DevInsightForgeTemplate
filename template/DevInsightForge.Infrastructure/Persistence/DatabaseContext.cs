using DevInsightForge.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace DevInsightForge.Infrastructure.Persistence;

public class DatabaseContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);

    }
}
