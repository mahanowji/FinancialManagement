using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class Invoice : BaseEntity
{
    public string InvoiceNumber { get; set; } = null!;

    public decimal Amount { get; set; }

    public InvoiceStatus Status { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; } = null!;

    public ICollection<PaymentRecord> Payments { get; set; } = new List<PaymentRecord>();
}