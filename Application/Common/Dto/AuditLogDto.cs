public class AuditLogDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Action { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}