using Edc.Core.AccountContext.Entities;

namespace Edc.Core.AccountContext.UseCases.Create.Interfaces;

public interface IAddRepository {
    Task AddAsync(Account account, CancellationToken cancellationToken);
}