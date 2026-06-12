using Domain.Interfaces;
using Domain.IRepository;

namespace Domain.Abstractions;

public interface IUnitOfWork
{
    public ISqlConnectionFactory SqlConnectionFactory { get; }
    public IUserRepository UserRepository { get; }
    public IAuditLogRepository AuditLogRepository { get; }
    public IJwtRepository JwtRepository{ get; }
    public IClientRepository ClientRepository{ get; }
    public IServicePlanRepository ServicePlanRepository { get; }
    public IConsentRepository ConsentRepository { get; }

    Task<bool> CommitChangesAsync();
    Task DisposeAsync();
    Task BeginTransactionAsync();
    Task RollbackTransactionAsync();
    Task CommitTransactionAsync();
    Task RejectChangesAsync();
}