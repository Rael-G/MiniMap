using System;
using Microsoft.Extensions.DependencyInjection;

namespace MiniMap
{
    /// <summary>
    /// Provides extension methods for registering the mapper service in the dependency injection container.
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Adds and configures the <see cref="MapperService"/> in the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which the mapper service will be added.</param>
        /// <param name="configure">A configuration action to customize the mapper service.</param>
        public static void AddMapper(this IServiceCollection services, Func<MapperConfigurator, MapperConfigurator> configure)
        {
            services.AddSingleton<IMapper>(provider =>
            {
                return configure(new MapperConfigurator()).Configure();
            });
        }
    }
}
