using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Enums;
using deposito_do_pitty.Domain.Interfaces;

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
            var now = DateTime.UtcNow;

            var entity = new AccountsPayable
            {
                Supplier = dto.Supplier,
                Description = dto.Description,
                Amount = dto.Amount,
                DueDate = dto.DueDate,
                PaymentDate = dto.PaymentDate,

                CreatedAt = now,
                UpdatedAt = now
            };

            ApplyStatusRules(entity, now);

            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(int id, AccountsPayableDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new Exception("Conta não encontrada.");

            var now = DateTime.UtcNow;

            entity.Supplier = dto.Supplier;
            entity.Description = dto.Description;
            entity.Amount = dto.Amount;
            entity.DueDate = dto.DueDate;
            entity.PaymentDate = dto.PaymentDate;

            ApplyStatusRules(entity, now);

            entity.UpdatedAt = now;

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }


        private static void ApplyStatusRules(AccountsPayable entity, DateTime nowUtc)
        {
            if (entity.PaymentDate.HasValue)
            {
                entity.Status = AccountsPayableStatus.Paid;
                entity.IsOverdue = false;
                return;
            }

            var overdueNow = nowUtc.Date > entity.DueDate.Date;

            entity.Status = overdueNow
                ? AccountsPayableStatus.Overdue
                : AccountsPayableStatus.Pending;

            entity.IsOverdue = overdueNow;
        }
    }
}
