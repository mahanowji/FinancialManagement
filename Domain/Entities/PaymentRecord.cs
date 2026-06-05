using CmsKit.Domain.Abstractions;
using FinancialAdvisor.Domain.Enums;

namespace FinancialAdvisor.Domain.Entities;

public class PaymentRecord : BaseEntity
{
    public decimal Amount { get; set; }

    public PaymentStatus Status { get; set; }

    public DateTime? PaidAt { get; set; }

    public Guid InvoiceId { get; set; }

    public Invoice Invoice { get; set; } = null!;
}