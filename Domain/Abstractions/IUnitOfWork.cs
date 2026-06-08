using Domain.IRepository;

namespace Domain.Abstractions;

public interface IUnitOfWork
{
    public ISqlConnectionFactory SqlConnectionFactory { get; }
    public IUserRepository UserRepository { get; }
    public IJwtRepository JwtRepository{ get; }

    Task<bool> CommitChangesAsync();
    Task DisposeAsync();
    Task BeginTransactionAsync();
    Task RollbackTransactionAsync();
    Task CommitTransactionAsync();
    Task RejectChangesAsync();
}