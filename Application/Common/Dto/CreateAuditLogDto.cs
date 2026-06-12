// CreateAuditLogDto.cs
public class CreateAuditLogDto
{
    public string Action { get; set; } = null!;
    public Guid UserId { get; set; }
}