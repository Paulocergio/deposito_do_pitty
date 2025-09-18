using deposito_do_pitty.Domain.Entities;


namespace DepositoDoPitty.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeactivateAsync(int id);
        Task<User?> GetByValidationEmailAsync(string email);
    }
}
