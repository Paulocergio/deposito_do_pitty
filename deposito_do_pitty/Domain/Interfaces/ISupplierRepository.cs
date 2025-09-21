using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Domain.Interfaces
{
    public interface ISupplierRepository
    {
        Task AddAsync(Supplier supplier);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
    }
}
