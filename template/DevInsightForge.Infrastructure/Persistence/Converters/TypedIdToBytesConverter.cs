using DevInsightForge.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DevInsightForge.Infrastructure.Persistence.Converters;

internal sealed class TypedIdToBytesConverter<TTypedIdValue>(ConverterMappingHints? mappingHints = null) : ValueConverter<TTypedIdValue, byte[]>(id => id.Value.ToByteArray(), value => Create(value), mappingHints)
        where TTypedIdValue : BaseTypedId
{
    private static TTypedIdValue Create(byte[] id)
    {
        TTypedIdValue instance = Activator.CreateInstance(typeof(TTypedIdValue), new Ulid(id)) as TTypedIdValue ??
            throw new Exception("Failed to convert from byte data");

        return instance;
    }


}
