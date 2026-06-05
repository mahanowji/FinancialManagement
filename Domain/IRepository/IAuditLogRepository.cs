using Domain.Entities;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log);

    Task<List<AuditLog>>
        GetAllAsync();
}