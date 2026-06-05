using Domain.Abstractions;

public interface ITaskService
{
    Task<ServiceResult<Guid>>
        CreateAsync(CreateTaskDto dto);

    Task<ServiceResult>
        CompleteAsync(Guid taskId);

    Task<ServiceResult<List<TaskDto>>>
        GetByClientIdAsync(Guid clientId);
}