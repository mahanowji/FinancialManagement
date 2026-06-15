using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistants.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
    }

    public async Task<List<TaskItem>> GetByClientIdAsync(Guid clientId)
    {
        return await _context.Tasks
            .Where(x => x.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks
            .FirstOrDefaultAsync(x => x.Id == id);
    }


    public async Task<int> GetPendingCountAsync()
    {
  
        return await _context.Tasks
            .CountAsync(t => !t.IsCompleted);
    }

    public async Task<int> GetOverdueCountAsync()
    {

        return await _context.Tasks
            .CountAsync(t => !t.IsCompleted && t.DueDate < DateTime.UtcNow);
    }

    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }
}