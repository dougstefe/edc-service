using System.Security.Cryptography;
using System.Text;

namespace Edc.Core.SharedContext.ValueObjects;

public class Password : ValueObject {
    public Password(string password) {
        ArgumentNullException.ThrowIfNullOrEmpty(password);

        Hash = Encrypt(password);
    }

    protected Password() {}

    public string Hash { get; private set; } = String.Empty;
    public string ResetCode { get; } = Guid.NewGuid().ToString("N")[..18].ToUpper();

    public bool Compare(string plainTextPassword) => Hash.Equals(Encrypt(plainTextPassword));

    public static implicit operator string(Password password) => password.ToString();
    public static implicit operator Password(string hash) => new() { Hash = hash };

    public override string ToString() => Hash;

    private string Encrypt(string text) {
        using SHA256 sha256 = SHA256.Create();
        text = String.Concat(text, Config.Secrets.PasswordSalt);
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
        StringBuilder stringBuilder = new();
        foreach (var hashByte in hashBytes) {
            stringBuilder.Append(hashByte.ToString());
        }
        return stringBuilder.ToString();
    }
}