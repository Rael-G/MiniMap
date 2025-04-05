using System;
using System.Collections.Generic;

namespace MiniMap
{
    /// <summary>
    /// Class responsible for managing and applying multiple mapping configurations.
    /// </summary>
    public class MapperConfigurator
    {
        private readonly List<IMapperConfig> _configs = new List<IMapperConfig>();

        /// <summary>
        /// Adds a mapping configuration using a generic type.
        /// The specified type must have a parameterless constructor.
        /// </summary>
        /// <typeparam name="T">Type that implements <see cref="IMapperConfig"/>.</typeparam>
        /// <returns>Returns the same <see cref="MapperConfigurator"/> instance for chaining.</returns>
        public MapperConfigurator Add<T>()
            where T : IMapperConfig, new()
        {
            var config = new T();
            return Add(config);
        }

        /// <summary>
        /// Adds a mapping configuration from a runtime type.
        /// The type must implement <see cref="IMapperConfig"/> and have a parameterless constructor.
        /// </summary>
        /// <param name="configType">The type of the configuration to be added.</param>
        /// <returns>Returns the same <see cref="MapperConfigurator"/> instance for chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the type does not implement <see cref="IMapperConfig"/>.</exception>
        public MapperConfigurator Add(Type configType)
        {
            if (!typeof(IMapperConfig).IsAssignableFrom(configType))
                throw new InvalidOperationException($"{configType.Name} does not implement IMapperConfig");

            var config = (IMapperConfig)Activator.CreateInstance(configType)!;
            return Add(config);
        }

        /// <summary>
        /// Adds an already created instance of a mapping configuration.
        /// </summary>
        /// <param name="config">Instance of the configuration to be added.</param>
        /// <returns>Returns the same <see cref="MapperConfigurator"/> instance for chaining.</returns>
        public MapperConfigurator Add(IMapperConfig config)
        {
            _configs.Add(config);
            return this;
        }

        /// <summary>
        /// Applies all added configurations and returns a ready-to-use instance of <see cref="MapperService"/>.
        /// </summary>
        /// <returns>A configured instance of <see cref="MapperService"/>.</returns>
        internal MapperService Configure()
        {
            var mapper = new MapperService();
            foreach (var config in _configs)
            {
                config.Configure(mapper);
            }

            return mapper;
        }
    }
}