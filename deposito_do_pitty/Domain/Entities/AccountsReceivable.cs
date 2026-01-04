using deposito_do_pitty.Domain.Enums;

namespace deposito_do_pitty.Domain.Entities
{
    

    public class AccountsReceivable
    {
        public int Id { get; set; }

        public string Customer { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime? ReceiptDate { get; set; }

        public AccountsReceivableStatus Status { get; set; } = AccountsReceivableStatus.Pendente;
        public bool IsOverdue { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
