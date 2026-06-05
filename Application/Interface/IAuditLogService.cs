using Domain.Abstractions;

public interface IAuditLogService
{
    Task<ServiceResult>
        CreateAsync(
            Guid userId,
            string action);

    Task<ServiceResult<List<AuditLogDto>>>
        GetAllAsync();
}