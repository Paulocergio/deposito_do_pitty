using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Domain.Interfaces
{
    public interface IAccountsPayableRepository
    {
        Task<IEnumerable<AccountsPayable>> GetAllAsync();
        Task<AccountsPayable?> GetByIdAsync(int id);
        Task AddAsync(AccountsPayable entity);
        Task UpdateAsync(AccountsPayable entity);
        Task DeleteAsync(int id);
    }
}
