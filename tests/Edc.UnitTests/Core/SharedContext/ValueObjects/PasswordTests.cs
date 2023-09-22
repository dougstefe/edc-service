
using Edc.Core;
using Edc.Core.SharedContext.ValueObjects;
using Xunit;

namespace Edc.UnitTests.Core.SharedContext.ValueObjects;

public class PasswordTests {

    [Fact]
    public void ShouldSuccessWithCorrectValue() {
        new Password("@nyP@55w0rd");
    }

    [Fact]
    public void ShouldUseImplictOperator() {
        var password = new Password("@nyP@55w0rd");
        var hash = password.Hash;

        Assert.Equal(hash, password);
    }

    [Fact]
    public void ShouldReturnTrueWhenComparePasswordsWithCorrectValue() {
        var plainTextPassword = "@nyP@55w0rd";
        var password = new Password(plainTextPassword);

        var isEquals = password.Compare(plainTextPassword);

        Assert.True(isEquals);
    }

    [Fact]
    public void ShouldReturnFalseWhenComparePasswordsWithIncorrectValue() {
        var password = new Password("@nyP@55w0rd");
        var invalidPassword = "anyP@55w0rd";

        var isEquals = password.Compare(invalidPassword);

        Assert.False(isEquals);
    }

    [Fact]
    public void ShouldReturnFalseWhenComparePasswordsWithCorrectValue() {
        var plainTextPassword = "@nyP@55w0rd";
        var password = new Password(plainTextPassword);
        Config.Secrets.PasswordSalt = "ChangePasswordSalt";

        var isEquals = password.Compare(plainTextPassword);

        Assert.False(isEquals);
    }
}
