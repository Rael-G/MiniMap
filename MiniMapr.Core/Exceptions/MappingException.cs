namespace MiniMapr.Core.Exceptions;

/// <summary>
/// Represents errors that occur during the mapping process in the MiniMapr framework.
/// This exception serves as the base class for more specific mapping-related exceptions.
/// </summary>
[Serializable]
public class MappingException : Exception
{

    /// <summary>
    /// Initializes a new instance of the <see cref="MappingException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public MappingException(string? message) : base(message)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MappingException"/> class with a specified 
    /// error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or a null reference.</param>
    public MappingException(string? message, Exception inner) : base(message, inner)
    {
        
    }
}
