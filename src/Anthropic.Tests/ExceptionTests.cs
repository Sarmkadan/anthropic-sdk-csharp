using Anthropic.Exceptions;
using Xunit;

namespace Anthropic.Tests;

public class ExceptionTests
{
    [Fact]
    public void ConfigurationException_ShouldWork()
    {
        var ex = new ConfigurationException("test");
        Assert.Equal("test", ex.Message);
    }

    [Fact]
    public void ValidationException_ShouldWork()
    {
        var ex = new ValidationException("test");
        Assert.Equal("test", ex.Message);
    }

    [Fact]
    public void MessageService_Constructor_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Anthropic.Services.MessageService(null!));
    }
}
