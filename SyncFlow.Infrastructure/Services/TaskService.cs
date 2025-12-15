using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.Common.Identity;
using SyncFlow.Application.Common.Models;
using SyncFlow.Application.Common.Notifications;
using SyncFlow.Application.DTOs.Tasks;
using SyncFlow.Application.Interfaces;
using SyncFlow.Infrastructure.Common;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities = SyncFlow.Domain.Entities;

namespace SyncFlow.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ISyncFlowDbContext _context;
    private readonly INotificationService _notify;
    private readonly ICurrentUser _current;
    public TaskService(ISyncFlowDbContext context, INotificationService notify, ICurrentUser current)
    {
        _context = context;
        _current = current;
        _notify = notify;
    }

    public async Task<PagedResult<TaskResponse>> GetAllAsync(TaskFilter filter)
    {
        if (filter == null)
        {
            throw new ArgumentNullException($"{nameof(filter)} is required");
        }

        var query = _context.Tasks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Title))
            query = query.Where(t => t.Title.Contains(filter.Title));

        if (filter.StartDate.HasValue)
            query = query.Where(t => t.StartDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(t => t.EndDate <= filter.EndDate.Value);

       var tasks= query
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                PhaseId = t.PhaseId,
                AssignedToId = t.AssignedToId,
                StartDate = t.StartDate,
                EndDate = t.EndDate
            });

        return await tasks.ToPagedResultAsync(filter);
    }

    public async Task<TaskResponse?> GetByIdAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return null;

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            PhaseId = task.PhaseId,
            AssignedToId = task.AssignedToId,
            StartDate = task.StartDate,
            EndDate = task.EndDate
        };
    }

    public async Task<TaskResponse> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = new Domain.Entities.Task
        {
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            PhaseId = request.PhaseId,
            StatusId = request.StatusId,
            AssignedToId = request.AssignedToId,
            StartDate = request.StartDate,
            EndDate = request.EndDate
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync(cancellationToken);

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            PhaseId = task.PhaseId,
            AssignedToId = task.AssignedToId,
            StartDate = task.StartDate,
            EndDate = task.EndDate
        };
    }

    public async Task<TaskResponse?> UpdateAsync(Guid id, UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.Include(s => s.Status).FirstOrDefaultAsync(s => s.Id == id);
        if (task == null) return null;

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;
        task.PhaseId = request.PhaseId;
        task.AssignedToId = request.AssignedToId;
        task.StartDate = request.StartDate;
        task.EndDate = request.EndDate;
        task.StatusId = request.StatusId;

        await _context.SaveChangesAsync(cancellationToken);

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            PhaseId = task.PhaseId,
            AssignedToId = task.AssignedToId,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            StatusId = task.StatusId,
            StatusName = task.Status?.Name
        };
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task AssignTaskAsync(Guid taskId, Guid? userId, Guid? roleId, CancellationToken cancellationToken)
    {
        if (userId == null && roleId == null)
            throw new ArgumentException("Debe especificar un usuario o un rol.");

        var assignment = new Entities.TaskAssignment
        {
            TaskId = taskId,
            UserId = userId,
            RoleId = roleId
        };

        _context.TaskAssignments.Add(assignment);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
