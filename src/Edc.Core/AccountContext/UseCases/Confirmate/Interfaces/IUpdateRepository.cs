using Edc.Core.AccountContext.Entities;

namespace Edc.Core.AccountContext.UseCases.Confirmate.Interfaces;

public interface IUpdateRepository {
    Task UpdateAsync(Account account, CancellationToken cancellationToken);
}
