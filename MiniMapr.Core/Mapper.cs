using System.Reflection;
using MiniMapr.Core;
using MiniMapr.Core.Exceptions;

namespace MiniMapr;

/// <summary>
/// A generic mapper that maps properties from a source object to a destination object.
/// Supports custom mappings, ignored properties, and value transformations.
/// </summary>
/// <typeparam name="TSource">The type of the source object.</typeparam>
/// <typeparam name="TDestination">The type of the destination object.</typeparam>
public class Mapper<TSource, TDestination> : ITypeMapper<TSource, TDestination>
    where TDestination : new()
{
    private readonly MapperOptions _mapperOptions;

    private readonly PropertyCache _propertyCache;

    public Mapper()
    {
        _mapperOptions = new MapperOptions();
        _propertyCache = new PropertyCache();
    }

    public Mapper(MapperOptions mapperOptions)
    {
        _mapperOptions = mapperOptions;
        _propertyCache = new PropertyCache();
    }

    /// <summary>
    /// Maps the properties from a source object to a newly created destination object.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <returns>A new instance of <typeparamref name="TDestination"/> with mapped properties.</returns>
    public virtual TDestination Map(TSource source)
    {
        var destination = new TDestination();
        return Map(source, destination);
    }

    /// <summary>
    /// Maps the properties from a source object to an existing destination object.
    /// </summary>
    /// <param name="source">The source object.</param>
    /// <param name="destination">The destination object.</param>
    /// <returns>The updated destination object.</returns>
    public virtual TDestination Map(TSource source, TDestination destination)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (destination is null) throw new ArgumentNullException(nameof(destination));

        var sourceProperties = _propertyCache.GetCachedProperties(typeof(TSource));
        var destinationProperties = _propertyCache.GetCachedProperties(typeof(TDestination))
            .ToDictionary(p => p.Name);

        foreach (var sourceProp in sourceProperties)
        {
            if (_mapperOptions.IgnoredProperties.Contains(sourceProp.Name))
                continue;

            var destPropName = GetDestinationPropertyName(sourceProp.Name);
            if (!destinationProperties.TryGetValue(destPropName, out var destProp))
                continue;

            if (!destProp.CanWrite)
            {
                throw new MappingException(
                    $"Property '{destProp.Name}' on type '{typeof(TDestination).Name}' is not writable.");
            }

            var setter = _propertyCache.GetSetter(destProp);
            var getter = _propertyCache.GetGetter(sourceProp);

            var sourceValue = GetTransformedValue(sourceProp.Name, getter(source));
            var convertedValue = ConvertValue(sourceValue, sourceProp.PropertyType, destProp.PropertyType, sourceProp.Name, destProp.Name);

            setter(destination, convertedValue);
        }

        return destination;
    }

    private static object? ConvertValue(object? value, Type sourceType, Type targetType, string sourcePropName, string destinationPropName)
    {
        if (value == null) return null;
        if (targetType.IsAssignableFrom(sourceType)) return value;

        try
        {
            return Convert.ChangeType(value, targetType);
        }
        catch (Exception ex)
        {
            throw new MappingConversionException(
                sourcePropName,
                destinationPropName,
                typeof(TSource).Name,
                typeof(TDestination).Name,
                value,
                targetType,
                ex);
        }
    }

    private string GetDestinationPropertyName(string sourcePropName)
    {
        return _mapperOptions.CustomMappings.TryGetValue(sourcePropName, out var mappedName)
            ? mappedName
            : sourcePropName;
    }

    private object? GetTransformedValue(string sourcePropName, object? originalValue)
    {
        return _mapperOptions.CustomTransformations.TryGetValue(sourcePropName, out var transform)
            ? transform(originalValue)
            : originalValue;
    }
}
