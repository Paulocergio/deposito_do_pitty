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

        public async Task<List<Budget>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Budget?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Budget?> GetByBudgetNumberAsync(string budgetNumber)
        {
            return await _repository.GetByBudgetNumberAsync(budgetNumber);
        }

        public async Task<List<Budget>> SearchByCustomerAsync(string customerName)
        {
            return await _repository.SearchByCustomerAsync(customerName);
        }

        public async Task<Budget> CreateAsync(Budget budget)
        {
            await _repository.AddAsync(budget);
            return budget;
        }

        public async Task<Budget> UpdateAsync(int id, Budget budget)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Orçamento não encontrado.");

       
            existing.CustomerName = budget.CustomerName;
            existing.Email = budget.Email;
            existing.Phone = budget.Phone;
            existing.Address = budget.Address;
            existing.IssueDate = budget.IssueDate;
            existing.DueDate = budget.DueDate;
            existing.Discount = budget.Discount;
            existing.Total = budget.Total;
            existing.Items.Clear();
            foreach (var item in budget.Items)
                existing.Items.Add(item);

            await _repository.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var budget = await _repository.GetByIdAsync(id);
            if (budget == null)
                return false;

            await _repository.DeleteAsync(budget);
            return true;
        }

        public async Task DeleteItemAsync(int itemId)
        {
            await _repository.DeleteItemAsync(itemId);
        }
    }
}
