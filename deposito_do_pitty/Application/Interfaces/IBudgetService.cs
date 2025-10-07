using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Application.Interfaces
{
    public interface IBudgetService
    {
        Task<List<Budget>> GetAllAsync();
        Task<Budget?> GetByIdAsync(int id);
        Task<Budget> CreateAsync(Budget budget);
        Task<Budget> UpdateAsync(int id, Budget budget);
        Task<bool> DeleteAsync(int id);               
        Task DeleteItemAsync(int itemId);        
        Task<List<Budget>> SearchByCustomerAsync(string customerName);
        Task<Budget?> GetByBudgetNumberAsync(string budgetNumber);
    }
}
