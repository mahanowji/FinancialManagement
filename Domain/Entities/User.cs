using Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public UserRole Role { get; set; }

    public ICollection<Client> Clients { get; set; } = new List<Client>();

    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}