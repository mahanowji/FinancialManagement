using Domain.Entities;

namespace Domain.IRepository
{
    public interface IJwtRepository
    {
        string GenerateToken(User user);
    }
}
