
using Domain.Entities;
namespace Domain.Interfaces
{
    public interface IServicePlanRepository
    {
        Task<ServicePlan?> GetByIdAsync(Guid id);
        Task<ServicePlan?> GetByNameAsync(string name);
        Task<List<ServicePlan>> GetAllAsync();
        Task<List<ServicePlan>> GetActiveAsync();
        Task AddAsync(ServicePlan servicePlan);
        Task UpdateAsync(ServicePlan servicePlan);
        Task DeleteAsync(ServicePlan servicePlan);
        Task<bool> ExistsAsync(Guid id);
    }
}