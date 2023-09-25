using Edc.Core.AccountContext.Entities;

public interface IGetByIdRepository {
    Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}