namespace deposito_do_pitty.Domain.Entities
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }

        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    
        public decimal Total { get; set; }

        public Budget? Budget { get; set; }
    }
}
