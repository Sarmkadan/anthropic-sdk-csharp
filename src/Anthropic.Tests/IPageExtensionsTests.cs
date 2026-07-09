using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anthropic.Core;
using Moq;
using Xunit;

namespace Anthropic.Tests;

public class IPageExtensionsTests
{
    [Fact]
    public async Task Paginate_YieldsAllItemsFromAllPages()
    {
        // Arrange
        var mockPage1 = new Mock<IPage<string>>();
        var mockPage2 = new Mock<IPage<string>>();
        var mockPage3 = new Mock<IPage<string>>();

        mockPage1.Setup(p => p.Items).Returns(new List<string> { "item1", "item2" });
        mockPage1.Setup(p => p.HasNext()).Returns(true);
        mockPage1.Setup(p => p.Next(TestContext.Current.CancellationToken)).ReturnsAsync(mockPage2.Object);

        mockPage2.Setup(p => p.Items).Returns(new List<string> { "item3", "item4" });
        mockPage2.Setup(p => p.HasNext()).Returns(true);
        mockPage2.Setup(p => p.Next(TestContext.Current.CancellationToken)).ReturnsAsync(mockPage3.Object);

        mockPage3.Setup(p => p.Items).Returns(new List<string> { "item5" });
        mockPage3.Setup(p => p.HasNext()).Returns(false);

        // Act
        var results = new List<string>();
        await foreach (var item in mockPage1.Object.Paginate(TestContext.Current.CancellationToken))
        {
            results.Add(item);
        }

        // Assert
        Assert.Equal(new List<string> { "item1", "item2", "item3", "item4", "item5" }, results);
    }
}
