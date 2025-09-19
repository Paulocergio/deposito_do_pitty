using deposito_do_pitty.Application.DTOs;
using DepositoDoPitty.Application.DTOs;

namespace DepositoDoPitty.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task CreateAsync(UserDto dto);
        Task UpdateAsync(UserDto dto);
        Task DeactivateAsync(int id);
     
    }
}
