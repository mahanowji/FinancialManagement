using CmsKit.Domain.IRepositories;
using CmsKit.Domain.IRepositories.UserContext;

namespace CmsKit.Domain.Abstractions;

public interface IUnitOfWork
{

    public ISqlConnectionFactory SqlConnectionFactory { get; }
    public IPostRepository PostRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserContext UserContext { get; }
    public IRoleUserRepository RoleUserRepository { get; }


    Task<bool> CommitChangesAsync();
    Task DisposeAsync();
    Task BeginTransactionAsync();
    Task RollbackTransactionAsync();
    Task CommitTransactionAsync();
    Task RejectChangesAsync();
}