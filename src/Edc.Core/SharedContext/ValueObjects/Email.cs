using System.Text.RegularExpressions;
using Edc.Core.Exceptions;

namespace Edc.Core.SharedContext.ValueObjects;

public partial class Email : ValueObject {
    private const string PATTERN = "^\\S+@\\S+\\.\\S+$";
    protected Email() {}
    public Email(string address) {
        ArgumentNullException.ThrowIfNullOrEmpty(address);

        Address = address;

        if (!EmailRegex().IsMatch(Address)) {
            throw new DomainException($"The prop '{nameof(Address)}' is invalid.");
        }

        VerificationCode = new();
    }
    public string Address { get; } = null!;
    public VerificationCode VerificationCode { get; private set; } = null!;

    public static implicit operator string(Email email) => email.ToString();
    public static implicit operator Email(string address) => new(address);

    public override string ToString() => Address;

    public void RecreateVerificationCode() => VerificationCode = new();

    [GeneratedRegex(PATTERN)]
    private static partial Regex EmailRegex();
}