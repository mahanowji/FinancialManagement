
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistants.Repositories
{
    public class ServicePlanRepository : IServicePlanRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicePlanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServicePlan?> GetByIdAsync(Guid id)
        {
            return await _context.ServicePlans
                .FirstOrDefaultAsync(sp => sp.Id == id);
        }

        public async Task<ServicePlan?> GetByNameAsync(string name)
        {
            return await _context.ServicePlans
                .FirstOrDefaultAsync(sp => sp.Name == name);
        }

        public async Task<List<ServicePlan>> GetAllAsync()
        {
            return await _context.ServicePlans
                .OrderBy(sp => sp.Name)
                .ToListAsync();
        }

        public async Task<List<ServicePlan>> GetActiveAsync()
        {
            return await _context.ServicePlans
                .OrderBy(sp => sp.Name)
                .ToListAsync();
        }

        public async Task AddAsync(ServicePlan servicePlan)
        {
            await _context.ServicePlans.AddAsync(servicePlan);
        }

        public Task UpdateAsync(ServicePlan servicePlan)
        {
            _context.ServicePlans.Update(servicePlan);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(ServicePlan servicePlan)
        {
            _context.ServicePlans.Remove(servicePlan);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.ServicePlans.AnyAsync(sp => sp.Id == id);
        }
    }
}