namespace DevInsightForge.Domain.Entities.Common;

public class BaseTypedId : IEquatable<BaseTypedId>
{
    public Ulid Value { get; }
    private readonly int _hashCode;

    public BaseTypedId(Ulid value)
    {
        Value = value;
        _hashCode = Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        return obj is BaseTypedId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }

    public bool Equals(BaseTypedId? other) => Value == other?.Value;

    public static bool operator ==(BaseTypedId obj1, BaseTypedId obj2)
    {
        if (ReferenceEquals(obj1, obj2)) return true;
        if (obj1 is null || obj2 is null) return false;
        return obj1.Equals(obj2);
    }

    public static bool operator !=(BaseTypedId x, BaseTypedId y)
    {
        return !(x == y);
    }
}
