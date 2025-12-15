using SyncFlow.Application.Common.Models;
using SyncFlow.Application.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces;

public interface ITaskService
{
    Task<PagedResult<TaskResponse>> GetAllAsync(TaskFilter filter);
    Task<TaskResponse?> GetByIdAsync(Guid id);
    Task<TaskResponse> CreateAsync(CreateTaskRequest request, CancellationToken cancellationToken);
    Task<TaskResponse?> UpdateAsync(Guid id, UpdateTaskRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
