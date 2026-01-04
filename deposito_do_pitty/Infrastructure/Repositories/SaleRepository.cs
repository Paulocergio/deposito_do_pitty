using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;
using DepositoDoPitty.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace deposito_do_pitty.Infrastructure.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly AppDbContext _context;

        public SaleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            return await _context.Sales
                .Include(s => s.items)
                .OrderByDescending(s => s.id)
                .ToListAsync();
        }

        public async Task<Sale?> GetByIdAsync(int id)
        {
            return await _context.Sales
                .Include(s => s.items)
                .FirstOrDefaultAsync(s => s.id == id);
        }

        public async Task AddAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
        }

        public Task UpdateAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Sale sale)
        {
            _context.Sales.Remove(sale);
            return Task.CompletedTask;
        }
    }
}
