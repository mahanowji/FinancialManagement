using Domain.Enums;

public class InvoiceDto
{
    public Guid Id { get; set; }

    public string InvoiceNumber { get; set; } = null!;

    public decimal Amount { get; set; }

    public InvoiceStatus Status { get; set; }
}