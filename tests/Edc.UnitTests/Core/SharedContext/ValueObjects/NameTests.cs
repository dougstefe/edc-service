using Edc.Core.SharedContext.ValueObjects;
using Xunit;

namespace Edc.UnitTests.Core.SharedContext.ValueObjects;

public class NameTests {
    
    [Fact]
    public void ShouldSuccessWithCorrectValue() {
        new Name("Any Name");
    }
    
    [Fact]
    public void ShouldUseImplictOperator() {
        var name = new Name("Any Name");
        var expected = "Any Name";

        Assert.Equal(expected, name);
    }

    [Fact]
    public void ShouldThrowArgumentExceptionWhenNameIsEmpty()
    {
        var task = () => new Name("");

        Assert.Throws<ArgumentException>(task);
    }
}