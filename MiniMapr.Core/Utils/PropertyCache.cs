using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace MiniMapr.Core.Utils;

/// <summary>
/// Provides cached access to property metadata and compiled getters/setters for improved performance during mapping.
/// </summary>
public class PropertyCache
{
    private readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();
    private readonly ConcurrentDictionary<string, Func<object, object?>> _getterCache = new();
    private readonly ConcurrentDictionary<string, Action<object, object?>> _setterCache = new();

    /// <summary>
    /// Gets the cached public instance properties of a given type. If not cached, they are retrieved via reflection and stored.
    /// </summary>
    /// <param name="type">The type whose properties should be retrieved.</param>
    /// <returns>An array of <see cref="PropertyInfo"/> representing the public instance properties of the type.</returns>
    public PropertyInfo[] GetCachedProperties(Type type)
    {
        if (!_propertyCache.TryGetValue(type, out var props))
        {
            props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _propertyCache[type] = props;
        }
        return props;
    }

    /// <summary>
    /// Creates a compiled getter delegate for a given property.
    /// </summary>
    /// <param name="property">The property for which to create the getter.</param>
    /// <returns>A delegate that takes an object instance and returns the value of the property.</returns>
    public static Func<object, object?> CreateGetter(PropertyInfo property)
    {
        var instance = Expression.Parameter(typeof(object), "instance");
        var castedInstance = Expression.Convert(instance, property.DeclaringType!);
        var propertyAccess = Expression.Property(castedInstance, property);
        var castResult = Expression.Convert(propertyAccess, typeof(object));
        var lambda = Expression.Lambda<Func<object, object?>>(castResult, instance);
        return lambda.Compile();
    }

    /// <summary>
    /// Creates a compiled setter delegate for a given property.
    /// </summary>
    /// <param name="property">The property for which to create the setter.</param>
    /// <returns>A delegate that takes an object instance and a value, and sets the property to the given value.</returns>
    public static Action<object, object?> CreateSetter(PropertyInfo property)
    {
        var instance = Expression.Parameter(typeof(object), "instance");
        var value = Expression.Parameter(typeof(object), "value");
        var castedInstance = Expression.Convert(instance, property.DeclaringType!);
        var castedValue = Expression.Convert(value, property.PropertyType);
        var propertyAccess = Expression.Property(castedInstance, property);
        var assign = Expression.Assign(propertyAccess, castedValue);
        var lambda = Expression.Lambda<Action<object, object?>>(assign, instance, value);
        return lambda.Compile();
    }

    /// <summary>
    /// Gets a cached or newly created getter delegate for the specified property.
    /// </summary>
    /// <param name="prop">The property for which to get the getter delegate.</param>
    /// <returns>A delegate that retrieves the value of the property from a given object.</returns>
    public Func<object, object?> GetGetter(PropertyInfo prop)
    {
        var key = $"{prop.DeclaringType!.FullName}.{prop.Name}";
        if (!_getterCache.TryGetValue(key, out var getter))
        {
            getter = CreateGetter(prop);
            _getterCache[key] = getter;
        }
        return getter;
    }

    /// <summary>
    /// Gets a cached or newly created setter delegate for the specified property.
    /// </summary>
    /// <param name="prop">The property for which to get the setter delegate.</param>
    /// <returns>A delegate that sets the value of the property on a given object.</returns>
    public Action<object, object?> GetSetter(PropertyInfo prop)
    {
        var key = $"{prop.DeclaringType!.FullName}.{prop.Name}";
        if (!_setterCache.TryGetValue(key, out var setter))
        {
            setter = CreateSetter(prop);
            _setterCache[key] = setter;
        }
        return setter;
    }
}
