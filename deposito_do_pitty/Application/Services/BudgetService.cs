using deposito_do_pitty.Application.DTOs.Budgets;
using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;

namespace deposito_do_pitty.Application.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _repository;

        public BudgetService(IBudgetRepository repository)
        {
            _repository = repository;
        }

        private static void Recalculate(Budget budget)
        {
            foreach (var item in budget.Items)
            {
                item.Total = item.Quantity * item.UnitPrice;
            }

            var subtotal = budget.Items.Sum(i => i.Total);
            var total = subtotal - budget.Discount + budget.Tax;

            budget.Total = total < 0 ? 0 : total;
            budget.UpdatedAt = DateTime.UtcNow;
        }

        public async Task<List<Budget>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Budget?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<Budget?> GetByBudgetNumberAsync(string budgetNumber)
            => await _repository.GetByBudgetNumberAsync(budgetNumber);

        public async Task<List<Budget>> SearchByCustomerAsync(string customerName)
            => await _repository.SearchByCustomerAsync(customerName);

        public async Task<Budget> CreateAsync(Budget budget)
        {
            Recalculate(budget);
            await _repository.AddAsync(budget);
            return budget;
        }

        public async Task<Budget> UpdateAsync(int id, Budget budget)
        {
            budget.Id = id;
            Recalculate(budget);
            await _repository.UpdateAsync(budget);
            return (await _repository.GetByIdAsync(id))!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var budget = await _repository.GetByIdAsync(id);
            if (budget == null) return false;

            await _repository.DeleteAsync(budget);
            return true;
        }

        public async Task DeleteItemAsync(int itemId) => await _repository.DeleteItemAsync(itemId);
    }
}
