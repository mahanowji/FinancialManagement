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
    public ICommunicationRepository CommunicationRepository { get; }

    public ITaskRepository TaskRepository { get; }  
    public IDocumentRepository DocumentRepository { get; }
    public IInvoiceRepository InvoiceRepository { get; }
    public IPaymentRepository PaymentRepository { get; }

    public IHouseHoldRepository HouseholdRepository { get; }    
    public INotificationEventRepository NotificationEventRepository { get; }    

    Task<bool> CommitChangesAsync();
    Task DisposeAsync();
    Task BeginTransactionAsync();
    Task RollbackTransactionAsync();
    Task CommitTransactionAsync();
    Task RejectChangesAsync();
}