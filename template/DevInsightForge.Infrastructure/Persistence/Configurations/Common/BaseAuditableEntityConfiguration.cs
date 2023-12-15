using DevInsightForge.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevInsightForge.Infrastructure.Persistence.Configurations.Common;

public abstract class BaseAuditableEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase>
where TBase : BaseAuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.HasKey(t => t.Id);

        builder.HasOne(t => t.CreatedByUser)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy)
            .IsRequired();

        builder.HasOne(t => t.UpdatedByUser)
            .WithMany()
            .HasForeignKey(t => t.ModifiedBy)
            .IsRequired();
    }
}
