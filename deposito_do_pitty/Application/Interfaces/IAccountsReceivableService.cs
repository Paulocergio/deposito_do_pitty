using deposito_do_pitty.Application.DTOs;

namespace deposito_do_pitty.Application.Interfaces
{
    public interface IAccountsReceivableService
    {
        Task CreateAsync(AccountsReceivableDto dto);
        Task UpdateAsync(int id, AccountsReceivableDto dto);
        Task DeleteAsync(int id);

        Task<AccountsReceivableDto?> GetByIdAsync(int id);
        Task<IEnumerable<AccountsReceivableDto>> GetAllAsync();
    }
}
