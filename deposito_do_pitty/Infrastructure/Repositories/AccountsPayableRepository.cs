using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;
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
                .AsNoTracking()
                .OrderByDescending(a => a.DueDate)
                .ToListAsync();
        }

        public async Task<AccountsPayable?> GetByIdAsync(int id)
        {
            return await _context.AccountsPayables
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(AccountsPayable entity)
        {
           
            if (entity.CreatedAt == default)
                entity.CreatedAt = DateTime.UtcNow;

            entity.UpdatedAt = DateTime.UtcNow;

            await _context.AccountsPayables.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AccountsPayable entity)
        {
        
            var existing = await _context.AccountsPayables
                .FirstOrDefaultAsync(a => a.Id == entity.Id);

            if (existing == null)
                throw new InvalidOperationException($"AccountsPayable Id={entity.Id} não encontrado.");

           
            existing.Supplier = entity.Supplier;
            existing.Description = entity.Description;
            existing.Amount = entity.Amount;
            existing.DueDate = entity.DueDate;
            existing.Status = entity.Status;
            existing.PaymentDate = entity.PaymentDate; 
            existing.IsOverdue = entity.IsOverdue;  

       
            existing.UpdatedAt = DateTime.UtcNow;

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
