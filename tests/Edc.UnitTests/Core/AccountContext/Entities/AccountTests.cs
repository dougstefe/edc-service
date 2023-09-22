using Edc.Core.AccountContext.Entities;
using Edc.Core.SharedContext.ValueObjects;
using Xunit;

namespace Edc.UnitTests.Core.AccountContext.Entities;

public class AccountTests {
    [Fact]
    public void ShouldCreateAccountInstance() {
        var name = new Name("Any Name");
        var email = new Email("any@email.com");
        var password = new Password("@nyP@55w0rd");
        var role = new Role("any_role", "any role description.");

        new Account(name, email, password, role);
    }
    
    [Fact]
    public void ShouldUpdatePassword() {
        var name = new Name("Any Name");
        var email = new Email("any@email.com");
        var password = new Password("@nyP@55w0rd");
        var role = new Role("any_role", "any role description.");
        var account = new Account(name, email, password, role);
        var resetCode = password.ResetCode;

        account.UpdatePassword("N3wP@55w0rd", resetCode);

        Assert.True(account.Password.Compare("N3wP@55w0rd"));
    }
    
    [Fact]
    public void ShouldUpdateEmail() {
        var name = new Name("Any Name");
        var email = new Email("any@email.com");
        var password = new Password("@nyP@55w0rd");
        var role = new Role("any_role", "any role description.");
        var account = new Account(name, email, password, role);

        account.UpdateEmail("new@email.com");

        Assert.Equal("new@email.com", account.Email);
    }
}