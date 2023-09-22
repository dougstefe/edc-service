using Xunit;
using Edc.Core.SharedContext.ValueObjects;
using Edc.Core.Exceptions;

namespace Edc.UnitTests.Core.SharedContext.ValueObjects;

public class EmailTests {
    
    [Fact]
    public void ShouldSuccessWithCorrectValue() {
        new Email("any_email@email.com");
    }

    [Fact]
    public void ShouldUseImplictOperator()
    {
        var email = new Email("any_email@email.com");
        var expected = "any_email@email.com";

        Assert.Equal(expected, email);
    }

    [Fact]
    public void ShouldThrowArgumentExceptionWhenEmailAddressIsEmpty()
    {
        var task = () => new Email("");

        Assert.Throws<ArgumentException>(task);
    }

    [Fact]
    public void ShouldThrowDomainExceptionWhenEmailAddressIsNotValid()
    {
        var task = () => new Email("any_email@email");

        Assert.Throws<DomainException>(task);
    }
}
