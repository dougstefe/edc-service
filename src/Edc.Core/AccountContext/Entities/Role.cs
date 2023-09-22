using Edc.Core.SharedContext.Entities;

namespace Edc.Core.AccountContext.Entities;

public class Role : Entity {
    private List<Account> _accounts = new();
    protected Role() {}

    public Role(string name, string description)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name);
        ArgumentNullException.ThrowIfNullOrEmpty(description);
        Name = name;
        Description = description;
    }

    public string Name { get; init; } = String.Empty;
    public string Description { get; init; } = String.Empty;

    public IEnumerable<Account> Accounts => _accounts;
}