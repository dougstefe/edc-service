using Edc.Core.AccountContext.Entities;

namespace Edc.Core.AccountContext.UseCases.Login.Interfaces;

public interface IGetRepository {
    Task<Account?> GetAsync(string email, CancellationToken cancellationToken);
}