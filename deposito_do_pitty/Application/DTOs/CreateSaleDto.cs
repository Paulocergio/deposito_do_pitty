using deposito_do_pitty.Domain.Enums;

namespace deposito_do_pitty.Application.DTOs
{
    public class CreateSaleDto
    {
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime Date { get; set; }

        public PaymentType PaymentType { get; set; } 

        public decimal DiscountPercent { get; set; }
        public List<CreateSaleItemDto> Items { get; set; } = new();
    }
}
