using Edc.Core.Exceptions;
using Edc.Core.SharedContext.ValueObjects;
using Xunit;

namespace Edc.UnitTests.Core.SharedContext.ValueObjects;

public class VerificationCodeTests {
    [Fact]
    public void ShouldCreateVerificationCodeCode() {
        var verificationCode = new VerificationCode();

        Assert.NotEmpty(verificationCode.Code);
    }

    [Fact]
    public void ShouldCreateVerificationCodeExpiresAt() {
        var verificationCode = new VerificationCode();

        Assert.NotNull(verificationCode.ExpiresAt);
    }

    [Fact]
    public void ShouldVerifyWithSuccessWhenIsCorrectCode() {
        var verificationCode = new VerificationCode();
        var code = verificationCode.Code;
        
        var action = () => verificationCode.Verify(code);

        action();
    }

    [Fact]
    public void ShouldThrowDomainExceptionWhenCodeHasAlreadyBeenVerified() {
        var verificationCode = new VerificationCode();
        var code = verificationCode.Code;
        verificationCode.Verify(code);

        var action = () => verificationCode.Verify(code);

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void ShouldThrowDomainExceptionWhenCodeIsInvalid() {
        var verificationCode = new VerificationCode();
        var code = "other_code";

        var action = () => verificationCode.Verify(code);

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void ShouldSetVerifiedAtWhenVerifyCodeIsSuccess() {
        var verificationCode = new VerificationCode();
        var code = verificationCode.Code;

        verificationCode.Verify(code);

        Assert.NotNull(verificationCode.VerifiedAt);
    }

    [Fact]
    public void ShouldSetToNullExpiresAtWhenVerifyCodeIsSuccess() {
        var verificationCode = new VerificationCode();
        var code = verificationCode.Code;

        verificationCode.Verify(code);

        Assert.Null(verificationCode.ExpiresAt);
    }
}