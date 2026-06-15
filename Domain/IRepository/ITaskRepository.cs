using Domain.Entities;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);

    Task<TaskItem?> GetByIdAsync(Guid id);

    Task<List<TaskItem>>
        GetByClientIdAsync(Guid clientId);
    Task<int> GetPendingCountAsync();
    Task<int> GetOverdueCountAsync();
}