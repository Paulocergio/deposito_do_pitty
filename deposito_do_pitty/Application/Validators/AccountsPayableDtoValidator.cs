using deposito_do_pitty.Application.DTOs;
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
                .Must(s => s == 0 || s == 1)
                .WithMessage("Status deve ser 0 (Pendente) ou 1 (Pago).");
        }
    }
}
