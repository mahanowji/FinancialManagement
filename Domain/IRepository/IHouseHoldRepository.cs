using Domain.Entities;

namespace Domain.IRepository
{
    public interface IHouseHoldRepository
    {
        Task<List<Household>> GetAllAsync();

        Task AddAsync(Household household);
    }
}
