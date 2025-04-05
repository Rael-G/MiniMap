using System;
using System.Collections.Generic;

namespace MiniMap
{
    /// <summary>
    /// A service that manages and applies registered mappers for mapping objects between different types.
    /// </summary>
    public class MapperService : IMapper
    {
        /// <summary>
        /// Stores registered mappers keyed by source and destination types.
        /// </summary>
        private readonly Dictionary<(Type Source, Type Destination), object> _mappers = new Dictionary<(Type Source, Type Destination), object>();

        /// <summary>
        /// Registers a new instance of <see cref="Mapper{TSource, TDestination}"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        public void Add<TSource, TDestination>()
            where TDestination : new()
        {
            var mapper = new Mapper<TSource, TDestination>();
            Add(mapper);
        }

        /// <summary>
        /// Registers and configures a new instance of <see cref="Mapper{TSource, TDestination}"/>.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="options">An action to configure the mapper.</param>
        /// <remarks>
        /// This method allows adding a mapper with custom property mappings or transformations.
        /// </remarks>
        public void Add<TSource, TDestination>(Action<MapperOptions> options)
            where TDestination : new()
        {
            var mapperOptions = new MapperOptions();
            var mapper = new Mapper<TSource, TDestination>(mapperOptions);
            options(mapperOptions);
            Add(mapper);
        }

        /// <summary>
        /// Registers a custom mapper for a specific source and destination type.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="mapper">The custom mapper instance.</param>
        public void Add<TSource, TDestination>(ITypeMapper<TSource, TDestination> mapper)
        {
            var key = (typeof(TSource), typeof(TDestination));
            _mappers[key] = mapper;
        }

        /// <summary>
        /// Maps an object from the source type to the destination type using a registered mapper.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="source">The source object to map.</param>
        /// <returns>A new instance of <typeparamref name="TDestination"/> populated with mapped values.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no mapper is registered for the specified types.</exception>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            var key = (typeof(TSource), typeof(TDestination));

            if (_mappers.TryGetValue(key, out var mapper))
            {
                return ((ITypeMapper<TSource, TDestination>)mapper).Map(source);
            }

            throw new InvalidOperationException($"No mapper registered for {typeof(TSource)} to {typeof(TDestination)}");
        }

        /// <summary>
        /// Maps an object from the source type to an existing instance of the destination type using a registered mapper.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDestination">The destination type.</typeparam>
        /// <param name="source">The source object to map.</param>
        /// <param name="destination">The destination object to populate.</param>
        /// <returns>The populated <typeparamref name="TDestination"/> instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no mapper is registered for the specified types.</exception>
        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            var key = (typeof(TSource), typeof(TDestination));

            if (_mappers.TryGetValue(key, out var mapper))
            {
                return ((ITypeMapper<TSource, TDestination>)mapper).Map(source, destination);
            }

            throw new InvalidOperationException($"No mapper registered for {typeof(TSource)} to {typeof(TDestination)}");
        }
    }
}
