﻿using DevInsightForge.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevInsightForge.Infrastructure.Persistence.Configurations.Common;

public abstract class BaseEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase>
    where TBase : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        builder.HasKey(t => t.Id);
    }
}