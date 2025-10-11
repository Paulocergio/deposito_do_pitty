using deposito_do_pitty.Application.DTOs;

namespace deposito_do_pitty.Application.Interfaces
{
    public interface IAccountsPayableService
    {
        Task<IEnumerable<AccountsPayableDto>> GetAllAsync();
        Task CreateAsync(AccountsPayableDto dto);
        Task UpdateAsync(int id, AccountsPayableDto dto); 
        Task DeleteAsync(int id);
    }
}
