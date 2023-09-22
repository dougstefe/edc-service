namespace Edc.Core.AccountContext.UseCases.Login.Interfaces;

public interface IValidation {
    bool Validate(Request request);
}