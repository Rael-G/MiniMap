using System;

namespace MiniMapr.Core.Exceptions;

[Serializable]
public class MappingException : Exception
{
    public MappingException(string? message) : base(message)
    {
        
    }

    public MappingException(string? message, Exception inner) : base(message, inner)
    {
        
    }
}
