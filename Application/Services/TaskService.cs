using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditLogService _auditLogService;

    public TaskService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _auditLogService = auditLogService;
    }

    public async Task<ServiceResult<Guid>> CreateAsync(CreateTaskDto dto)
    {
        try
        {
         
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(dto.ClientId);
            if (client == null)
                return new ServiceResult<Guid>($"Client not found");


            var task = new Domain.Entities.TaskItem
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                IsCompleted = false,
                ClientId = dto.ClientId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.TaskRepository.AddAsync(task);
            await _unitOfWork.CommitChangesAsync();

      
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Task '{task.Title}' created for client {client.FirstName} {client.LastName}",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<Guid>(task.Id);
        }
        catch (Exception ex)
        {
            return new ServiceResult<Guid>($"Error creating task: {ex.Message}");
        }
    }

    public async Task<ServiceResult> CompleteAsync(Guid taskId)
    {
        try
        {
    
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(taskId);
            if (task == null)
                return new ServiceResult($"Task not found");

            if (task.IsCompleted)
                return new ServiceResult($"Task already completed");

        
            task.IsCompleted = true;
            task.CompletedAt = DateTime.Now;

            await _unitOfWork.TaskRepository.UpdateAsync(task);
            await _unitOfWork.CommitChangesAsync
                ();

    
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Task '{task.Title}' marked as completed",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult();
        }
        catch (Exception ex)
        {
            return new ServiceResult($"Error completing task: {ex.Message}");
        }
    }

    public async Task<ServiceResult<List<TaskDto>>> GetByClientIdAsync(Guid clientId)
    {
        try
        {

            var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
            if (client == null)
                return new ServiceResult<List<TaskDto>>($"Client not found");


            var tasks = await _unitOfWork.TaskRepository.GetByClientIdAsync(clientId);

 
            var dtos = tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                IsCompleted = t.IsCompleted
            }).ToList();

     
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Tasks viewed for client {client.FirstName} {client.LastName} ({dtos.Count} tasks)",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<List<TaskDto>>(dtos);
        }
        catch (Exception ex)
        {
            return new ServiceResult<List<TaskDto>>($"Error getting tasks: {ex.Message}");
        }
    }
}