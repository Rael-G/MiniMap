using System.Collections.Concurrent;
using System.Reflection;

namespace MiniMapr.Core;

public class PropertyCache
{
    private readonly ConcurrentDictionary<Type, PropertyInfo[]> _propertyCache = new();

    public PropertyInfo[] GetCachedProperties(Type type)
    {
        if (!_propertyCache.TryGetValue(type, out var props))
        {
            props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            _propertyCache[type] = props;
        }
        return props;
    }

}
