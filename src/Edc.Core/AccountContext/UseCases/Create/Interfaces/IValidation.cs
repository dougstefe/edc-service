namespace Edc.Core.AccountContext.UseCases.Create.Interfaces;
public interface IValidation {
    bool Validate(Request request);
}