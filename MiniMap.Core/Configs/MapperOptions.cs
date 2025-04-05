using System;
using System.Collections.Generic;

namespace MiniMap
{
    /// <summary>
    /// Represents configuration options for customizing the object mapping behavior,
    /// including custom property mappings, ignored properties, and transformation logic.
    /// </summary>
    public class MapperOptions
    {
        /// <summary>
        /// Stores custom property mappings where the key is the source property name
        /// and the value is the corresponding destination property name.
        /// </summary>
        public Dictionary<string, string> CustomMappings { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Stores the names of source properties that should be ignored during mapping.
        /// </summary>
        public HashSet<string> IgnoredProperties { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Stores custom transformations for specific properties, allowing source values
        /// to be modified before being assigned to the destination object.
        /// </summary>
        public Dictionary<string, Func<object?, object?>> CustomTransformations { get; private set; } = new Dictionary<string, Func<object?, object?>>();

        /// <summary>
        /// Adds a custom transformation for a specific property, converting its value from the source type to the destination type.
        /// </summary>
        /// <typeparam name="TSourceProperty">The type of the source property.</typeparam>
        /// <typeparam name="TDestinationProperty">The type of the destination property.</typeparam>
        /// <param name="property">The name of the property to apply the transformation to.</param>
        /// <param name="func">A function that takes a value of type <typeparamref name="TSourceProperty"/> 
        /// and returns a value of type <typeparamref name="TDestinationProperty"/>.</param>
        public void AddCustomTransformation<TSourceProperty, TDestinationProperty>(string property, Func<TSourceProperty, TDestinationProperty> func)
        {
            CustomTransformations[property] = obj => obj is TSourceProperty typedObj ? func(typedObj) : default!;
        }
    }
}
