using Edc.Core.AccountContext.Entities;
using Edc.Core.AccountContext.UseCases.Confirmate.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Edc.Infra.AccountContext.UseCases.Confirmate;

public class Repository : IGetByIdRepository, IUpdateRepository
{
    private readonly EdcDbContext _dbContext;

    public Repository(EdcDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _dbContext
            .Accounts
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken)
    {
        _dbContext.Update(account);
        await _dbContext.SaveChangesAsync();
    }
}