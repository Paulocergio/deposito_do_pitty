using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Domain.Entities;
using DepositoDoPitty.Application.DTOs;

namespace DepositoDoPitty.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<int> CreateAsync(UserDto dto); 
        Task UpdateAsync(UpdateUserDto dto);
        Task<User?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }

}
