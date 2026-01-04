using deposito_do_pitty.Domain.Enums;

namespace deposito_do_pitty.Application.DTOs
{
    public class SaleDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public DateTime Date { get; set; }

        public PaymentType PaymentType { get; set; }
        public SaleStatus Status { get; set; }

        public decimal Subtotal { get; set; }
        public decimal DiscountPercent { get; set; }

        // NO BANCO É discountvalue
        public decimal DiscountValue { get; set; }

        public decimal Total { get; set; }

        public List<SaleItemDto> Items { get; set; } = new();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
