using deposito_do_pitty.Domain.Entities;
using DepositoDoPitty.Application.DTOs;
using DepositoDoPitty.Application.Interfaces;
using BCrypt.Net;
using DepositoDoPitty.Domain.Interfaces;
using deposito_do_pitty.Application.DTOs;
namespace DepositoDoPitty.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role
            });
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };
        }

        public async Task CreateAsync(UserDto dto)
        {
            var existingEmail = await _userRepository.GetByValidationEmailAsync(dto.Email);
            if (existingEmail != null)
            {
                throw new InvalidOperationException("Email Ja Cadastrado");
            }
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive = true
            };

            await _userRepository.AddAsync(user);
        }

        public async Task UpdateAsync(UserDto dto)
        {
            var user = new User
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role
            };
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeactivateAsync(int id)
        {
            await _userRepository.DeactivateAsync(id);
        }


    }
}
