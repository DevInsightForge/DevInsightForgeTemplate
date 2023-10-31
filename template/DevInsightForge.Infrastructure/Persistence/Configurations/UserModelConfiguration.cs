using DevInsightForge.Domain.Entities.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DevInsightForge.Infrastructure.Persistence.Configurations;

public class UserModelConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.NormalizedEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.NormalizedEmail)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.IsEmailVerified)
            .IsRequired();

        builder.Property(u => u.DateJoined)
            .IsRequired();

        builder.Property(u => u.LastLogin)
            .IsRequired();
    }
}
