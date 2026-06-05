using CmsKit.Domain.Abstractions;

namespace FinancialAdvisor.Domain.Entities;

public class Consent : BaseEntity
{
    public string Description { get; set; } = null!;

    public bool Accepted { get; set; }

    public DateTime? AcceptedAt { get; set; }

    public Guid ClientId { get; set; }

    public Client Client { get; set; } = null!;
}