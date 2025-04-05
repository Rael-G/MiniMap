namespace MiniMapr
{
    /// <summary>
    /// Interface that must be implemented by classes responsible for configuring mappings.
    /// </summary>
    public interface IMapperConfig
    {
        /// <summary>
        /// Method called during the mapper configuration phase.
        /// Use this to register custom mappings in the <see cref="MapperService"/>.
        /// </summary>
        /// <param name="mapperService">Instance of the mapping service that will receive the configurations.</param>
        void Configure(MapperService mapperService);
    }
}

