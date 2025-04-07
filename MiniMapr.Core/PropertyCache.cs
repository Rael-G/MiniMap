using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace MiniMapr.Core;

public class PropertyCache
{
    private readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();

    private readonly ConcurrentDictionary<string, Func<object, object?>> _getterCache = new();

    private readonly ConcurrentDictionary<string, Action<object, object?>> _setterCache = new();

    public PropertyInfo[] GetCachedProperties(Type type)
    {
        if (!_propertyCache.TryGetValue(type, out var props))
        {
            props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _propertyCache[type] = props;
        }
        return props;
    }

    public static Func<object, object?> CreateGetter(PropertyInfo property)
    {
        var instance = Expression.Parameter(typeof(object), "instance");

        // (TargetType)instance
        var castedInstance = Expression.Convert(instance, property.DeclaringType!);

        // ((TargetType)instance).Property
        var propertyAccess = Expression.Property(castedInstance, property);

        // Convert the property value to object (if it's a value type)
        var castResult = Expression.Convert(propertyAccess, typeof(object));

        // Final lambda: instance => (object)((TargetType)instance).Property
        var lambda = Expression.Lambda<Func<object, object?>>(castResult, instance);

        return lambda.Compile(); // cria delegate compilado, muito mais rápido
    }

    public static Action<object, object?> CreateSetter(PropertyInfo property)
    {
        var instance = Expression.Parameter(typeof(object), "instance");
        var value = Expression.Parameter(typeof(object), "value");

        // (TargetType)instance
        var castedInstance = Expression.Convert(instance, property.DeclaringType!);

        // (PropertyType)value
        var castedValue = Expression.Convert(value, property.PropertyType);

        // ((TargetType)instance).Property = (PropertyType)value
        var propertyAccess = Expression.Property(castedInstance, property);
        var assign = Expression.Assign(propertyAccess, castedValue);

        var lambda = Expression.Lambda<Action<object, object?>>(assign, instance, value);
        return lambda.Compile();
    }

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
