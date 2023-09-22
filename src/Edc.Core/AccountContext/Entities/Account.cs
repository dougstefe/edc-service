using Edc.Core.Exceptions;
using Edc.Core.SharedContext.Entities;
using Edc.Core.SharedContext.ValueObjects;

namespace Edc.Core.AccountContext.Entities;

public class Account : Entity {

    private List<Role> _roles = new();

    protected Account() {}

    public Account(Email email, Password password)
    {
        Email = email;
        Password = password;
    }

    public Account(Name name, Email email, Password password, Role role)
    {
        Name = name;
        Email = email;
        Password = password;
        _roles.Add(role);
    }

    public Name Name { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;

    public Image? Image { get; private set; } = null;
    public IEnumerable<Role> Roles => _roles;

    public void UpdatePassword(string password, string code) {
        if (!string.Equals(code.Trim(), Password.ResetCode.Trim(), StringComparison.CurrentCultureIgnoreCase))
            throw new DomainException("Invalid reset code.");
        Password = new Password(password);
    }

    public void UpdateEmail(string email) {
        Email = new Email(email);
    }
}