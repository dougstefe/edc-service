using Edc.Core.AccountContext.Entities;
using Edc.Core.AccountContext.UseCases.Create.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Edc.Infra.AccountContext.UseCases.Create;

public class Repository : IAddRepository, IExistsRepository, IGetUserRoleRepository
{
    private readonly EdcDbContext _dbContext;

    private const string USER_ROLE_NAME = "User";

    public Repository(EdcDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken) {
        await _dbContext.AddAsync(account, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken) =>
        await _dbContext
            .Accounts
            .AsNoTracking()
            .AnyAsync(x => x.Email.Address == email, cancellationToken);

    public async Task<Role> GetUserRoleAsync(CancellationToken cancellationToken) =>
        await _dbContext
            .Roles
            .SingleAsync(x => x.Name == USER_ROLE_NAME, cancellationToken);
}