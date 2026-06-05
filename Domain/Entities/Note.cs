using CmsKit.Domain.Abstractions;

namespace FinancialAdvisor.Domain.Entities;

public class Note : BaseEntity
{
    public string Content { get; set; } = null!;

    public Guid ClientId { get; set; }

    public Client Client { get; set; } = null!;
}