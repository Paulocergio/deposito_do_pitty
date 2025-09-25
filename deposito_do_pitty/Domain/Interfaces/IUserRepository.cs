using deposito_do_pitty.Domain.Entities;


namespace DepositoDoPitty.Domain.Interfaces
{
    public interface IUserRepository
    {
       
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<User?> GetByValidationEmailAsync(string email);
    }
}
