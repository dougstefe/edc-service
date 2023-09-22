namespace Edc.Core.SharedContext.ValueObjects;

public class Name : ValueObject {
    protected Name() {}
    public Name(string fullName)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(fullName);
        FullName = fullName;
    }

    public string FullName { get; } = String.Empty;

    public static implicit operator string(Name name) => name.ToString();
    public static implicit operator Name(string fullName) => new(fullName);

    public override string ToString() => FullName;
}