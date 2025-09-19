using deposito_do_pitty.Application.DTOs;
using deposito_do_pitty.Application.DTOs.Auth;


namespace deposito_do_pitty.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> AuthenticateAsync(LoginRequestDto request);
    }
}
