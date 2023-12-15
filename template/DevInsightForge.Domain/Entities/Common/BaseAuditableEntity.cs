using DevInsightForge.Domain.Entities.Core;

namespace DevInsightForge.Domain.Entities.Common;

public abstract class BaseAuditableEntity(long createdBy) : BaseEntity
{
    public long CreatedBy { get; private set; } = createdBy;
    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    public long ModifiedBy { get; private set; } = createdBy;
    public DateTime ModifiedDate { get; private set; } = DateTime.UtcNow;

    #region Foreign Key Relation
    public virtual UserModel CreatedByUser { get; }
    public virtual UserModel UpdatedByUser { get; }
    #endregion

    public void UpdateAuditFields(long modifiedBy)
    {   
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public bool HasBeenModified() => CreatedDate != ModifiedDate;

}
