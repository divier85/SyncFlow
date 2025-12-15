using SyncFlow.Application.Common.Models;
using SyncFlow.Application.DTOs.Projects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services;

public interface IProjectService
{
    Task<PagedResult<ProjectResponse>> GetAllAsync(ProjectFilter filter);
    Task<ProjectResponse?> GetByIdAsync(Guid id);
    Task<ProjectResponse> CreateAsync(CreateProjectRequest dto, CancellationToken cancellationToken);
    Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
