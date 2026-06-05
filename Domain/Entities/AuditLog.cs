using Domain.Abstractions;

namespace Domain.Entities;

public class AuditLog : BaseEntity
{
    public string Action { get; set; } = null!;

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;
}