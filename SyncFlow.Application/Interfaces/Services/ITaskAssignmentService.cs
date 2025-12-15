using SyncFlow.Application.DTOs.TaskAssignments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services
{
    public interface ITaskAssignmentService
    {
        Task<TaskAssignmentResponse> AssignAsync(CreateTaskAssignmentRequest request, CancellationToken cancellationToken);
        Task<List<TaskAssignmentResponse>> GetByTaskIdAsync(Guid taskId);

        Task<TaskAssignmentResponse> UpdateAssignAsync(Guid id, UpdateTaskAssignmentRequest request, CancellationToken cancellationToken);
    }
}
