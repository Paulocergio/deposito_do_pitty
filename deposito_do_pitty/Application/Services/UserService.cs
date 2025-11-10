using deposito_do_pitty.Domain.Entities;
using DepositoDoPitty.Application.DTOs;
using DepositoDoPitty.Application.Interfaces;
using BCrypt.Net;
using DepositoDoPitty.Domain.Interfaces;
using deposito_do_pitty.Application.DTOs;
using Microsoft.AspNetCore.Identity;
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
                Role = u.Role,
                IsActive = u.IsActive 
            });
        }




        public async Task<int> CreateAsync(UserDto dto)
        {
            var existingEmail = await _userRepository.GetByValidationEmailAsync(dto.Email);
            if (existingEmail != null)
                throw new InvalidOperationException("Email Ja Cadastrado");

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
            return user.Id; 
        }


        public async Task UpdateAsync(UpdateUserDto dto)
        {
            var existingUser = await _userRepository.GetByIdAsync(dto.Id);
            if (existingUser == null)
                throw new Exception("Usuário não encontrado");

            existingUser.Name = dto.Name;
            existingUser.Email = dto.Email;
            existingUser.Phone = dto.Phone;
            existingUser.Role = dto.Role;
            existingUser.IsActive = dto.IsActive;
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            }

            await _userRepository.UpdateAsync(existingUser);
        }




        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
             return await _userRepository.GetByIdAsync(id);
           
        }
    }
}
