using Edc.Core.AccountContext.Entities;
using Edc.Core.AccountContext.UseCases.Login.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Edc.Infra.AccountContext.UseCases.Login;

public class Repository : IGetRepository
{
    private readonly EdcDbContext _dbContext;

    public Repository(EdcDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Account?> GetAsync(string email, CancellationToken cancellationToken) =>
        await _dbContext
            .Accounts
            .Include(x => x.Roles)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email.Address == email, cancellationToken);
}