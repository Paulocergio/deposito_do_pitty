using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Domain.Interfaces
{
    public interface IBudgetRepository
    {

        Task<List<Budget>> GetAllAsync();
        Task<Budget?> GetByIdAsync(int id);
        Task<Budget?> GetByBudgetNumberAsync(string budgetNumber);
        Task<List<Budget>> SearchByCustomerAsync(string customerName);

        Task AddAsync(Budget budget);
        Task UpdateAsync(Budget budget);
        Task DeleteAsync(Budget budget);

        Task DeleteItemAsync(int itemId);
    }
}
