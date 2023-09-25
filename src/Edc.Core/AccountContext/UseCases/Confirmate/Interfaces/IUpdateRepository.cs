using Edc.Core.AccountContext.Entities;

public interface IUpdateRepository {
    Task UpdateAsync(Account account, CancellationToken cancellationToken);
}
