using System.Data;

namespace Domain.Abstractions
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}