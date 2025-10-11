using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;
using deposito_do_pitty.Infrastructure.Persistence;
using DepositoDoPitty.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace deposito_do_pitty.Infrastructure.Repositories
{
    public class AccountsPayableRepository : IAccountsPayableRepository
    {
        private readonly AppDbContext _context;

        public AccountsPayableRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccountsPayable>> GetAllAsync()
        {
            return await _context.AccountsPayables
                // .Include(a => a.Supplier)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<AccountsPayable?> GetByIdAsync(int id)
        {
            return await _context.AccountsPayables
              
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(AccountsPayable entity)
        {
            await _context.AccountsPayables.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AccountsPayable entity)
        {
            
            var tracked = await _context.AccountsPayables.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == entity.Id);
            if (tracked == null)
                throw new InvalidOperationException($"AccountsPayable Id={entity.Id} não encontrado.");
            entity.UpdatedAt = DateTime.UtcNow;         
            _context.AccountsPayables.Attach(entity);
            _context.Entry(entity).Property(e => e.Description).IsModified = true;
            _context.Entry(entity).Property(e => e.Amount).IsModified = true;
            _context.Entry(entity).Property(e => e.DueDate).IsModified = true;
            _context.Entry(entity).Property(e => e.Status).IsModified = true;
            _context.Entry(entity).Property(e => e.UpdatedAt).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.AccountsPayables.FindAsync(id);
            if (existing == null) return;

            _context.AccountsPayables.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
