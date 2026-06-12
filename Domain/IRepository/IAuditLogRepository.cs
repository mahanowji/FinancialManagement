using Domain.Entities;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log);

    Task<List<AuditLog>>
        GetAllAsync();

    Task<List<AuditLog>> GetByUserIdAsync(Guid userId);
}