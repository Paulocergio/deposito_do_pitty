using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Domain.Enums;
using FluentValidation;

namespace deposito_do_pitty.Application.Validators
{
    public class AccountsPayableDtoValidator : AbstractValidator<AccountsPayableDto>
    {
        public AccountsPayableDtoValidator()
        {
            RuleFor(x => x.Supplier)
                .NotEmpty().WithMessage("O campo 'Supplier' é obrigatório.")
                .MaximumLength(255).WithMessage("O campo 'Supplier' deve ter no máximo 255 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(255).WithMessage("A descrição deve ter no máximo 255 caracteres.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("A data de vencimento é obrigatória.");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Status inválido.");

    
            RuleFor(x => x.PaymentDate)
                .NotNull()
                .When(x => x.Status == AccountsPayableStatus.Paid)
                .WithMessage("PaymentDate é obrigatório quando o status for Pago.");

       
            RuleFor(x => x.PaymentDate)
                .Null()
                .When(x => x.Status != AccountsPayableStatus.Paid)
                .WithMessage("PaymentDate deve ser nulo quando a conta não está paga.");
        }
    }
}
