using Domain.Abstractions;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistants
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;


        public UnitOfWork(ApplicationDbContext context,
            ISqlConnectionFactory sqlConnectionFactory,
            IUserRepository userRepository,
            IJwtRepository jwtRepository,
            IAuditLogRepository auditLogRepository,
            IClientRepository clientRepository,
            IServicePlanRepository servicePlanRepository,
            IConsentRepository consentRepository,
            ICommunicationRepository communicationRepository,
            IDocumentRepository documentRepository,
            IInvoiceRepository invoiceRepository,
            ITaskRepository taskRepository)

        {
            _context = context;
            SqlConnectionFactory = sqlConnectionFactory;
            UserRepository = userRepository;
            JwtRepository = jwtRepository;
            AuditLogRepository = auditLogRepository;
            ClientRepository = clientRepository;
            ServicePlanRepository = servicePlanRepository;
            ConsentRepository = consentRepository;
            CommunicationRepository = communicationRepository;
            DocumentRepository = documentRepository;
            InvoiceRepository = invoiceRepository;
            TaskRepository = taskRepository;    
        }


        public ISqlConnectionFactory SqlConnectionFactory { get; }
        public IUserRepository UserRepository { get; }
        public IJwtRepository JwtRepository { get; }
        public IAuditLogRepository AuditLogRepository { get; }
        public IClientRepository ClientRepository { get; }

        public IServicePlanRepository ServicePlanRepository { get; }
        public IConsentRepository ConsentRepository { get; }

        public ICommunicationRepository CommunicationRepository { get; }

        public IDocumentRepository DocumentRepository { get; }

        public IInvoiceRepository InvoiceRepository { get; } 
        
        public ITaskRepository TaskRepository { get; }  

        #region functionalities

        public async Task<bool> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync(true, CancellationToken.None) > 0;
        }

        public async Task DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RejectChangesAsync()
        {
            foreach (var entry in _context.ChangeTracker.Entries()
                         .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                        await entry.ReloadAsync();
                        break;
                    case EntityState.Deleted:
                        await entry.ReloadAsync();
                        break;
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}
