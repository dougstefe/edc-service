using Edc.Core.Exceptions;

namespace Edc.Core.SharedContext.ValueObjects;

public class VerificationCode : ValueObject {
    public string Code {get;} = Guid.NewGuid().ToString("N")[..6].ToUpper();
    public DateTime? ExpiresAt {get; private set;} = DateTime.UtcNow.AddMinutes(5);
    public DateTime? VerifiedAt { get; private set; } = null;
    public bool IsActive => VerifiedAt is not null;

    public void Verify(string code){
        if (IsActive)
            throw new DomainException("This account is already active");
        
        if (ExpiresAt < DateTime.UtcNow)
            throw new DomainException("This code has already expired.");
        
        if (!string.Equals(Code, code, StringComparison.CurrentCultureIgnoreCase))
            throw new DomainException("This code is invalid.");
        
        VerifiedAt = DateTime.UtcNow;
        ExpiresAt = null;
    }
}