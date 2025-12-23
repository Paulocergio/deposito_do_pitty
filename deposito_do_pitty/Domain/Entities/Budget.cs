namespace deposito_do_pitty.Domain.Entities
{
    public class Budget
    {
        public int Id { get; set; }

        public string BudgetNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
        public string? Address { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }

        public decimal Discount { get; set; }
        public decimal Tax { get; set; }    

        public decimal Total { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<BudgetItem> Items { get; set; } = new List<BudgetItem>();
    }
}
