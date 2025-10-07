namespace deposito_do_pitty.Application.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public string? Category { get; set; }
        public int StockQuantity { get; set; }
        public string Status { get; set; } = "ATIVO";
        public string Barcode { get; set; } = null!;
    }
}
