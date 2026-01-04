using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Domain.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<Sale>> GetAllAsync();
        Task<Sale?> GetByIdAsync(int id);
        Task AddAsync(Sale sale);
        Task UpdateAsync(Sale sale);
        Task DeleteAsync(Sale sale);
    }
}
