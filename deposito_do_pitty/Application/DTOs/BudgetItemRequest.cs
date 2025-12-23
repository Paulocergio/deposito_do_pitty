using System.Text.Json.Serialization;

namespace deposito_do_pitty.Application.DTOs.Budgets
{
    public class BudgetItemRequest
    {
        public int? Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}