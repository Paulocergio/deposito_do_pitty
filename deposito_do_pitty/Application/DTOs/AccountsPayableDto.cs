using deposito_do_pitty.Domain.Enums;

public class AccountsPayableDto
{
    public int Id { get; set; }
    public string Supplier { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }

    public AccountsPayableStatus Status { get; set; }

    public DateTime? PaymentDate { get; set; }
    public bool IsOverdue { get; set; }
}
