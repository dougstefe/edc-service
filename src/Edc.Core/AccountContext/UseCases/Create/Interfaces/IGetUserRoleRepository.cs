
using Edc.Core.AccountContext.Entities;

namespace Edc.Core.AccountContext.UseCases.Create.Interfaces;

public interface IGetUserRoleRepository {
    Task<Role> GetUserRoleAsync(CancellationToken cancellationToken);
}