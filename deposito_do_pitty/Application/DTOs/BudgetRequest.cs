using deposito_do_pitty.Application.DTOs.Budgets;

namespace deposito_do_pitty.Application.DTOs
{
    public class BudgetRequest
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }

        public List<BudgetItemRequest> Items { get; set; } = new();

        public decimal Discount { get; set; }
        public decimal Tax { get; set; }

        public decimal? Total { get; set; }

        public string BudgetNumber { get; set; } = string.Empty;
    }
}
