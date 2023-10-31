using DevInsightForge.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevInsightForge.Infrastructure.Persistence.Converters;

internal sealed class TypedIdToBytesConverter<TTypedIdValue> : ValueConverter<TTypedIdValue, byte[]>
        where TTypedIdValue : BaseTypedId
{
    public TypedIdToBytesConverter(ConverterMappingHints? mappingHints = null)
        : base(id => id.Value.ToByteArray(), value => Create(value), mappingHints)
    {
    }

    private static TTypedIdValue Create(byte[] id)
    {
        TTypedIdValue instance = Activator.CreateInstance(typeof(TTypedIdValue), new Ulid(id)) as TTypedIdValue ??
            throw new Exception("Failed to convert from byte data");

        return instance;
    }


}
