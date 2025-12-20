namespace deposito_do_pitty.Domain.Entities
{
    public sealed class RegisterRequest
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string Password { get; set; } = "";
    }
}
