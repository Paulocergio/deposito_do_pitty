using DepositoDoPitty.Application.DTOs;
using FluentValidation;
namespace deposito_do_pitty.Application.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {

        public UserDtoValidator() {
            RuleFor(u => u.Email).NotEmpty().MaximumLength(5);
            
        }
    }
}
