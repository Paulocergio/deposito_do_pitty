namespace deposito_do_pitty.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty; 
        public string? Phone { get; set; }
        public string? Email { get; set; }   
        public string? Address { get; set; }    
        public string? PostalCode { get; set; }
        public string? ContactPerson { get; set; }
        public bool IsActive { get; set; } = true;  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
