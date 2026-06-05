using Domain.Enums;

public class PaymentDto
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    public DateTime? PaidAt { get; set; }
}