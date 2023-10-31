using DevInsightForge.Domain.Entities.Common;

namespace DevInsightForge.Domain.Entities.User;

public sealed class UserId : BaseTypedId
{
    public UserId(Ulid value) : base(value)
    {
    }

    public UserId() : base(Ulid.NewUlid())
    {
    }
}
