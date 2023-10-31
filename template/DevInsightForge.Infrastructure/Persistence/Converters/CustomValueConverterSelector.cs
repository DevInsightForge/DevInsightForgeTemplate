using DevInsightForge.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Concurrent;

namespace DevInsightForge.Infrastructure.Persistence.Converters;

/// <summary>
/// Based on https://andrewlock.net/strongly-typed-ids-in-ef-core-using-strongly-typed-entity-ids-to-avoid-primitive-obsession-part-4/
/// </summary>
internal sealed class CustomValueConverterSelector : ValueConverterSelector
{
    private readonly ConcurrentDictionary<(Type ModelClrType, Type ProviderClrType), ValueConverterInfo> _converters
        = new();

    public CustomValueConverterSelector(ValueConverterSelectorDependencies dependencies)
        : base(dependencies)
    {
    }

    public override IEnumerable<ValueConverterInfo> Select(Type modelClrType, Type? providerClrType = null)
    {
        var baseConverters = base.Select(modelClrType, providerClrType);
        foreach (var converter in baseConverters)
        {
            yield return converter;
        }

        var underlyingModelType = UnwrapNullableType(modelClrType);
        var underlyingProviderType = UnwrapNullableType(providerClrType);

        if (underlyingProviderType is null || underlyingProviderType == typeof(Ulid))
        {
            var isTypedIdValue = typeof(BaseTypedId).IsAssignableFrom(underlyingModelType);
            if (underlyingModelType is not null && isTypedIdValue)
            {
                var converterType = typeof(TypedIdToBytesConverter<>).MakeGenericType(underlyingModelType);

                if (Activator.CreateInstance(converterType, new ConverterMappingHints()) is ValueConverter converter)
                {
                    yield return _converters.GetOrAdd((underlyingModelType, typeof(Ulid)), _ =>
                    {
                        return new ValueConverterInfo(
                            modelClrType,
                            typeof(byte[]),
                            info => Activator.CreateInstance(converterType, info.MappingHints) as ValueConverter ?? converter);
                    });
                }
            }
        }
    }

    private static Type? UnwrapNullableType(Type? type)
    {
        if (type is null)
        {
            return null;
        }

        return Nullable.GetUnderlyingType(type) ?? type;
    }
}
