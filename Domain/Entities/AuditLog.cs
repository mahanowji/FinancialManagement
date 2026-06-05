using CmsKit.Domain.Abstractions;

namespace FinancialAdvisor.Domain.Entities;

public class AuditLog : BaseEntity
{
    public string Action { get; set; } = null!;

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}