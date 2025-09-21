using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Interfaces;
using deposito_do_pitty.Application.Interfaces;
using DepositoDoPitty.Application.DTOs;

namespace deposito_do_pitty.Application.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task AddSupplierAsync(Supplier supplier)
        {
            supplier.CreatedAt = DateTime.UtcNow;
            supplier.UpdatedAt = DateTime.UtcNow;

            await _supplierRepository.AddAsync(supplier);
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _supplierRepository.GetAllAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _supplierRepository.GetByIdAsync(id);
        }

        public async Task UpdateSupplierAsync(int id, Supplier supplier)
        {
            var existing = await _supplierRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException("Supplier not found");
            existing.CompanyName = supplier.CompanyName;
            existing.Address = supplier.Address;
            existing.Number = supplier.Number;
            existing.Neighborhood = supplier.Neighborhood;
            existing.City = supplier.City;
            existing.State = supplier.State;
            existing.PostalCode = supplier.PostalCode;
            existing.Phone = supplier.Phone;
            existing.RegistrationStatus = supplier.RegistrationStatus;
            existing.BranchType = supplier.BranchType;
            existing.Email = supplier.Email;
            existing.UpdatedAt = DateTime.UtcNow;

            await _supplierRepository.UpdateAsync(existing);
        }

        public async Task DeleteSupplierAsync(int id)
        {
            await _supplierRepository.DeleteAsync(id);
        }
    }
}
