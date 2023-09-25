namespace Edc.Core.AccountContext.UseCases.Confirmate.Interfaces;

public interface IValidation {
    bool Validate(Request request);
}