using System;

namespace Anthropic.Exceptions;

public class ConfigurationException : AnthropicException
{
    public ConfigurationException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}
