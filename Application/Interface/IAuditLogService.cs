using Domain.Abstractions;

public interface IAuditLogService
{

    Task<ServiceResult> CreateAsync(CreateAuditLogDto dto);
    Task<ServiceResult<List<AuditLogDto>>> GetAllAsync();
    Task<ServiceResult<List<AuditLogDto>>> GetByUserIdAsync(Guid userId);
}