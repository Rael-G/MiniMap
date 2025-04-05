namespace MiniMapr;

/// <summary>
/// Defines a contract for mapping objects from one type to another.
/// </summary>
public interface IMapper
{
    /// <summary>
    /// Maps an object of type <typeparamref name="TSource"/> to a new instance of type <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The source object type.</typeparam>
    /// <typeparam name="TDestination">The destination object type.</typeparam>
    /// <param name="source">The source object to map from.</param>
    /// <returns>A new instance of <typeparamref name="TDestination"/> populated with mapped values from the source object.</returns>
    TDestination Map<TSource, TDestination>(TSource source);

    /// <summary>
    /// Maps an object of type <typeparamref name="TSource"/> to an existing instance of type <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The source object type.</typeparam>
    /// <typeparam name="TDestination">The destination object type.</typeparam>
    /// <param name="source">The source object to map from.</param>
    /// <param name="destination">The existing instance of <typeparamref name="TDestination"/> to populate.</param>
    /// <returns>The populated <typeparamref name="TDestination"/> instance.</returns>
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}
