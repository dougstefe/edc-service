using Edc.Core.AccountContext.Entities;

namespace Edc.Core.AccountContext.UseCases.Confirmate.Interfaces;

public interface IGetByIdRepository {
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}