using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Interfaces;
using deposito_do_pitty.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using deposito_do_pitty.Domain.Entities;

namespace deposito_do_pitty.Application.Services
{
    public class AccountsPayableService : IAccountsPayableService
    {
        private readonly IAccountsPayableRepository _repository;

        public AccountsPayableService(IAccountsPayableRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AccountsPayableDto>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();

            return items.Select(a => new AccountsPayableDto
            {
                Id = a.Id,
                Supplier = a.Supplier,
                Description = a.Description,
                Amount = a.Amount,
                DueDate = a.DueDate,
                Status = a.Status,
                PaymentDate = a.PaymentDate,   
                IsOverdue = a.IsOverdue        
            }); 
        }


        public async Task CreateAsync(AccountsPayableDto dto)
        {
            var entity = new AccountsPayable
            {
                Supplier = dto.Supplier,
                Description = dto.Description,
                Amount = dto.Amount,
                DueDate = dto.DueDate,
                Status = dto.Status,
                PaymentDate = dto.Status == 1 ? DateTime.UtcNow : null,
                IsOverdue = dto.Status == 0 && dto.DueDate < DateTime.UtcNow
            };

            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, AccountsPayableDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Conta não encontrada.");

            entity.Supplier = dto.Supplier;
            entity.Description = dto.Description;
            entity.Amount = dto.Amount;
            entity.DueDate = dto.DueDate;
            entity.Status = dto.Status;
            entity.PaymentDate = dto.Status == 1 ? DateTime.UtcNow : entity.PaymentDate;
            entity.IsOverdue = dto.Status == 0 && dto.DueDate < DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);
        }


        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

       
    }
}
