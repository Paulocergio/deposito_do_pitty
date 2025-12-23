using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;
using deposito_do_pitty.Infrastructure.Persistence;
using DepositoDoPitty.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace deposito_do_pitty.Infrastructure.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly AppDbContext _context;

        public BudgetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Budget>> GetAllAsync()
        {
            return await _context.Budgets
                .Include(b => b.Items)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }

        public async Task<Budget?> GetByIdAsync(int id)
        {
            return await _context.Budgets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Budget?> GetByBudgetNumberAsync(string budgetNumber)
        {
            return await _context.Budgets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.BudgetNumber == budgetNumber);
        }

        public async Task<List<Budget>> SearchByCustomerAsync(string customerName)
        {
            return await _context.Budgets
                .Include(b => b.Items)
                .Where(b => EF.Functions.ILike(b.CustomerName, $"%{customerName}%"))
                .ToListAsync();
        }

        public async Task AddAsync(Budget budget)
        {
            await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Budget budget)
        {
            var existing = await _context.Budgets
                .Include(b => b.Items)
                .FirstOrDefaultAsync(b => b.Id == budget.Id);

            if (existing == null)
                throw new InvalidOperationException("Orçamento não encontrado.");

            existing.BudgetNumber = budget.BudgetNumber;
            existing.CustomerName = budget.CustomerName;
            existing.Email = budget.Email;
            existing.Phone = budget.Phone;
            existing.Address = budget.Address;
            existing.IssueDate = budget.IssueDate;
            existing.DueDate = budget.DueDate;
            existing.Discount = budget.Discount;
            existing.Tax = budget.Tax;
            existing.UpdatedAt = DateTime.UtcNow;


            var incomingById = budget.Items
                .Where(i => i.Id != 0)
                .ToDictionary(i => i.Id);

            var toRemove = existing.Items.Where(ei => !incomingById.ContainsKey(ei.Id)).ToList();
            if (toRemove.Any())
                _context.BudgetItems.RemoveRange(toRemove);

            foreach (var item in budget.Items)
            {
                if (item.Id == 0)
                {
                    existing.Items.Add(new BudgetItem
                    {
                        Description = item.Description,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Total = item.Quantity * item.UnitPrice
                    });
                }
                else
                {
                    var tracked = existing.Items.First(ei => ei.Id == item.Id);
                    tracked.Description = item.Description;
                    tracked.Quantity = item.Quantity;
                    tracked.UnitPrice = item.UnitPrice;
                    tracked.Total = item.Quantity * item.UnitPrice;
                }
            }

            var subtotal = existing.Items.Sum(i => i.Total);
            var total = subtotal - existing.Discount + existing.Tax;
            existing.Total = total < 0 ? 0 : total;

            await _context.SaveChangesAsync();
        }



        public async Task DeleteAsync(Budget budget)
        {
            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int itemId)
        {
            var item = await _context.BudgetItems.FindAsync(itemId);
            if (item != null)
            {
                _context.BudgetItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteItemsByBudgetIdAsync(int budgetId)
        {
            var items = await _context.BudgetItems
                .Where(i => i.BudgetId == budgetId)
                .ToListAsync();

            if (items.Any())
            {
                _context.BudgetItems.RemoveRange(items);
                await _context.SaveChangesAsync();
            }
        }

    }
}
