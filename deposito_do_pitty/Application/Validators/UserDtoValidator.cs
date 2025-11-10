using DepositoDoPitty.Application.DTOs;
using FluentValidation;

namespace deposito_do_pitty.Application.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(120).WithMessage("Nome deve ter no máximo 120 caracteres.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.")
                .MaximumLength(254).WithMessage("E-mail deve ter no máximo 254 caracteres.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres.");

            RuleFor(u => u.Phone)
                .MaximumLength(30).When(u => !string.IsNullOrWhiteSpace(u.Phone))
                .WithMessage("Telefone deve ter no máximo 30 caracteres.");

           
        }
    }
}
