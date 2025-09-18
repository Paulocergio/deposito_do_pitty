using DepositoDoPitty.Domain.Enums;

namespace DepositoDoPitty.Application.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; } = null!;
    }
}
