using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Application.Interfaces
{
    public interface ISupplierService
    {
        Task AddSupplierAsync(Supplier supplier);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task UpdateSupplierAsync(int id, Supplier supplier);
        Task DeleteSupplierAsync(int id);
    }
}
