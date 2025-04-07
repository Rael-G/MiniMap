using MiniMapr.Core.Exceptions;

namespace MiniMapr;

/// <summary>
/// Exception thrown when a value cannot be converted during the mapping process between two types.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MappingConversionException"/> class with detailed information
/// about the mapping failure, including the property names, types involved, and the reason for the failure.
/// </remarks>
/// <param name="sourcePropertyName">The name of the source property that failed to convert.</param>
/// <param name="destinationPropertyName">The name of the destination property where the value was being mapped.</param>
/// <param name="sourceTypeName">The name of the source type in the mapping operation.</param>
/// <param name="destinationTypeName">The name of the destination type in the mapping operation.</param>
/// <param name="value">The value that failed to convert.</param>
/// <param name="targetType">The expected type of the destination property.</param>
/// <param name="innerException">The original exception thrown during the conversion attempt.</param>
[Serializable]
public class MappingConversionException(
    string sourcePropertyName,
    string destinationPropertyName,
    string sourceTypeName,
    string destinationTypeName,
    object? value,
    Type targetType,
    Exception innerException) : MappingException(
        $"Error mapping property '{sourcePropertyName}' (source type: {value?.GetType().Name ?? "null"}) " +
            $"to property '{destinationPropertyName}' (target type: {targetType.Name}) " +
            $"in mapping from {sourceTypeName} to {destinationTypeName}. " +
            $"Conversion failed: {innerException.Message}", 
        innerException)
{
}
