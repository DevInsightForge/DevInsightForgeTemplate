﻿using DevInsightForge.Domain.Entities.Core;

namespace DevInsightForge.Domain.Entities.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public Guid? CreatedByUserId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public Guid? ModifiedByUserId { get; private set; }
    public DateTime ModifiedOn { get; private set; }

    #region Foreign Key Relations
    public virtual UserModel? CreatedByUser { get; }
    public virtual UserModel? ModifiedByUser { get; }
    #endregion

    public void SetCreationAudit(Guid? createdByUserId)
    {
        CreatedByUserId = createdByUserId;
        CreatedOn = DateTime.UtcNow;
    }

    public void SetModificationAudit(Guid? modifiedByUserId)
    {
        ModifiedByUserId = modifiedByUserId;
        ModifiedOn = DateTime.UtcNow;
    }

    public bool HasBeenModified() => CreatedOn != ModifiedOn;
}
