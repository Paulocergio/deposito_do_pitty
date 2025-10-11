namespace deposito_do_pitty.Domain.Entities
{
    public class AccountsPayable
    {
        public int Id { get; set; }


        public string Supplier { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool IsOverdue { get; set; }
    }
}
