using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Application.Interfaces;
using deposito_do_pitty.Domain.Entities;
using deposito_do_pitty.Domain.Enums;
using DepositoDoPitty.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace deposito_do_pitty.Application.Services
{
    public class AccountsReceivableService : IAccountsReceivableService
    {
        private readonly AppDbContext _context;

        public AccountsReceivableService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(AccountsReceivableDto dto)
        {
            Validate(dto);

            var entity = new AccountsReceivable
            {
                Customer = dto.Customer.Trim(),
                Description = dto.Description.Trim(),
                Amount = dto.Amount,
                DueDate = AsUnspecified(dto.DueDate),
                ReceiptDate = AsUnspecified(dto.ReceiptDate),
                CreatedAt = AsUnspecified(DateTime.Now),
                UpdatedAt = null
            };

            ApplyBusinessRules(entity);

            _context.AccountsReceivable.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, AccountsReceivableDto dto)
        {
            Validate(dto);

            var entity = await _context.AccountsReceivable.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) throw new KeyNotFoundException("Conta a receber não encontrada.");

            entity.Customer = dto.Customer.Trim();
            entity.Description = dto.Description.Trim();
            entity.Amount = dto.Amount;

            entity.DueDate = AsUnspecified(dto.DueDate);
            entity.ReceiptDate = AsUnspecified(dto.ReceiptDate);

            entity.UpdatedAt = AsUnspecified(DateTime.Now);

            ApplyBusinessRules(entity);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.AccountsReceivable.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) return;

            _context.AccountsReceivable.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<AccountsReceivableDto?> GetByIdAsync(int id)
        {
            var e = await _context.AccountsReceivable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return e == null ? null : ToDto(e);
        }

        public async Task<IEnumerable<AccountsReceivableDto>> GetAllAsync()
        {
            var list = await _context.AccountsReceivable
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();

            return list.Select(ToDto);
        }

        private static AccountsReceivableDto ToDto(AccountsReceivable e) => new()
        {
            Id = e.Id,
            Customer = e.Customer,
            Description = e.Description,
            Amount = e.Amount,
            DueDate = e.DueDate,
            ReceiptDate = e.ReceiptDate,
            Status = (short)e.Status,
            IsOverdue = e.IsOverdue
        };

        private static void ApplyBusinessRules(AccountsReceivable e)
        {
         
            if (e.ReceiptDate.HasValue)
            {
                e.Status = AccountsReceivableStatus.Recebida;
                e.IsOverdue = false;
                return;
            }

    
            var today = DateTime.Today;
            var due = e.DueDate.Date;

            if (due < today)
            {
                e.Status = AccountsReceivableStatus.Atrasada;
                e.IsOverdue = true;
            }
            else
            {
                e.Status = AccountsReceivableStatus.Pendente;
                e.IsOverdue = false;
            }
        }

        private static void Validate(AccountsReceivableDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Customer))
                throw new ArgumentException("Customer é obrigatório.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ArgumentException("Description é obrigatório.");

            if (dto.Amount < 0)
                throw new ArgumentException("Amount não pode ser negativo.");
        }

        private static DateTime AsUnspecified(DateTime dt)
        {
           
            return DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);
        }

        private static DateTime? AsUnspecified(DateTime? dt)
        {
            return dt.HasValue ? AsUnspecified(dt.Value) : null;
        }
    }
}
