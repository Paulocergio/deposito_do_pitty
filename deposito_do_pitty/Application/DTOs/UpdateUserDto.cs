using DepositoDoPitty.Domain.Enums;

namespace deposito_do_pitty.Application.DTOs
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }   
        public string? Password { get; set; }
    }
}
