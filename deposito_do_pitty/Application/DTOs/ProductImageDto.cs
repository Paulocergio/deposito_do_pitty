namespace deposito_do_pitty.Application.DTOs
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
    }
}
