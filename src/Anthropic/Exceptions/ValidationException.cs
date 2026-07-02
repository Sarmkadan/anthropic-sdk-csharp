using System;

namespace Anthropic.Exceptions;

public class ValidationException : AnthropicException
{
    public ValidationException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}
