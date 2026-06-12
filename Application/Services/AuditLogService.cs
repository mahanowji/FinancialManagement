using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

public class AuditService : IAuditLogService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork unitOfWork;

    public AuditService(IHttpContextAccessor httpContextAccessor, IUnitOfWork _unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
       unitOfWork = _unitOfWork;
    }

    public async Task<ServiceResult> CreateAsync(CreateAuditLogDto dto)
    {
        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            Action = dto.Action,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.AuditLogRepository.AddAsync(auditLog);
        return new ServiceResult();
    }

    public async Task<ServiceResult<List<AuditLogDto>>> GetAllAsync()
    {
        var logs = await unitOfWork.AuditLogRepository.GetAllAsync();

        var dtos = new List<AuditLogDto>();

        foreach (var log in logs)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(log.UserId);
            dtos.Add(new AuditLogDto
            {
                Id = log.Id,
                Action = log.Action,
                UserId = log.UserId,
                UserFullName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown",
                CreatedAt = log.CreatedAt
            });
        }

        return new ServiceResult<List<AuditLogDto>>(dtos);
    }

    public async Task<ServiceResult<List<AuditLogDto>>> GetByUserIdAsync(Guid userId)
    {
        var logs = await unitOfWork.AuditLogRepository.GetByUserIdAsync(userId);

        var dtos = logs.Select(log => new AuditLogDto
        {
            Id = log.Id,
            Action = log.Action,
            UserId = log.UserId,
            UserFullName = "",
            CreatedAt = log.CreatedAt
        }).ToList();

        return new ServiceResult<List<AuditLogDto>>(dtos);
    }
}


