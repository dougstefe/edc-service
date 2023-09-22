using Edc.Core.AccountContext.Entities;

namespace Edc.Core.AccountContext.UseCases.Create.Interfaces;

public interface IService
{
    Task SendVerificationAsync(Account account, CancellationToken cancellationToken);
}