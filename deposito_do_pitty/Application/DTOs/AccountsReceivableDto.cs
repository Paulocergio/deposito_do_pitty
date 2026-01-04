namespace deposito_do_pitty.Application.DTOs
{
    public class AccountsReceivableDto
    {
        public int? Id { get; set; } 

        public string Customer { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime? ReceiptDate { get; set; }


        public short Status { get; set; } = 0;
        public bool IsOverdue { get; set; } = false;
    }
}
