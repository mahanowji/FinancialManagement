public class CreateInvoiceDto
{
    public Guid ClientId { get; set; }

    public decimal Amount { get; set; }

    public string InvoiceNumber { get; set; } = null!;
}