using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;
using DepositoDoPitty.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace deposito_do_pitty.Infrastructure.Repositories
{
    public class AccountsReceivableRepository : IAccountsReceivableRepository
    {
        private readonly AppDbContext _context;

        public AccountsReceivableRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AccountsReceivable entity)
        {
            await _context.AccountsReceivable.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AccountsReceivable entity)
        {
            _context.AccountsReceivable.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(AccountsReceivable entity)
        {
            _context.AccountsReceivable.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<AccountsReceivable?> GetByIdAsync(int id)
        {
            return await _context.AccountsReceivable
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AccountsReceivable>> GetAllAsync()
        {
            return await _context.AccountsReceivable
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }
}
