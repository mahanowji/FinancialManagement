public class AuditLogDto
{

    public Guid Id { get; set; }
    public string Action { get; set; } = null!;
    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

}