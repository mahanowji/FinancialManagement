public class CreatePaymentDto
{
    public Guid InvoiceId { get; set; }

    public decimal Amount { get; set; }
}