using SyncFlow.Application.Common.Models;
using SyncFlow.Application.DTOs.Process;
using SyncFlow.Application.DTOs.ProcessAssignments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services
{
    public interface IProcessService
    {
        Task<PagedResult<ProcessResponse>> GetAllAsync(ProcessFilter filter);
        Task<ProcessResponse?> GetByIdAsync(Guid id);
        Task<ProcessResponse> CreateAsync(CreateProcessRequest dto, CancellationToken cancellationToken);
        Task<ProcessResponse> UpdateAsync(Guid id, UpdateProcessRequest dto, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task<ProcessAssignmentResponse> AssignProcessAsync(CreateProcessAssignmentRequest request, CancellationToken cancellationToken);

        Task<IEnumerable<ProcessResponse>> GetProcessesByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<ProcessResponse>> GetProcessesByRoleAsync(Guid roleId, CancellationToken cancellationToken);
        Task<ProcessAssignmentResponse> UpdateProcessAssignmentAsync(Guid assignmentId, CreateProcessAssignmentRequest request, CancellationToken cancellationToken);
        Task<bool> RemoveProcessAssignmentAsync(Guid assignmentId, CancellationToken cancellationToken);

    }
}
