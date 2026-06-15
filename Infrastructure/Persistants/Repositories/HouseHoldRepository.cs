using Domain.Entities;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistants.Repositories
{
    internal class HouseHoldRepository : IHouseHoldRepository
    {
        private readonly ApplicationDbContext _context;

        public HouseHoldRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Household household)
        {
            await _context.Households.AddAsync(household);
        }

        public async Task<List<Household>> GetAllAsync()
        {
            return await _context.Households
                .OrderBy(u => u.Name)
                .ToListAsync();
        }
    }
}
