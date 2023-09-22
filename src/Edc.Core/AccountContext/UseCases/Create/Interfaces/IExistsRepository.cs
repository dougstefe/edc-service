
namespace Edc.Core.AccountContext.UseCases.Create.Interfaces;

public interface IExistsRepository {
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken);
}