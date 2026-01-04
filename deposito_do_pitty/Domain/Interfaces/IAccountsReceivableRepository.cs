using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Domain.Interfaces
{
    public interface IAccountsReceivableRepository
    {
        Task AddAsync(AccountsReceivable entity);
        Task UpdateAsync(AccountsReceivable entity);
        Task DeleteAsync(AccountsReceivable entity);

        Task<AccountsReceivable?> GetByIdAsync(int id);
        Task<List<AccountsReceivable>> GetAllAsync();
    }
}
