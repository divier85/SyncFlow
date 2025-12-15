using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.Common.Exceptions;
using SyncFlow.Application.Common.Identity;
using SyncFlow.Application.Common.Models;
using SyncFlow.Application.DTOs.Projects;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Domain.Entities;
using SyncFlow.Infrastructure.Common;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly ISyncFlowDbContext _context;
    private readonly ICurrentUser _current;

    public ProjectService(ISyncFlowDbContext context, ICurrentUser current)
    {
        _context = context;
        _current = current;
    }

    public async Task<PagedResult<ProjectResponse>> GetAllAsync(ProjectFilter filter)
    {
        if (filter == null)
            throw new ArgumentNullException(nameof(filter));

        var query = _context.Projects.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(p => p.Name.Contains(filter.Name));

        if (filter.StartDate.HasValue)
            query = query.Where(p => p.StartDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(p => p.EndDate <= filter.EndDate.Value);

        var projected = query.Select(p => new ProjectResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            BusinessId = p.BusinessId,
            CreatedAt = p.CreatedAt
        });

        return await projected.ToPagedResultAsync(filter);
    }

    public async Task<ProjectResponse?> GetByIdAsync(Guid id)
    {
        var p = await _context.Projects.FindAsync(id);
        if (p == null) return null;

        return new ProjectResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            StartDate = p.StartDate,
            EndDate = p.EndDate,
            BusinessId = p.BusinessId,
            CreatedAt = p.CreatedAt
        };
    }

    public async Task<ProjectResponse> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        if (!_current.IsInRole("Manager") && !_current.IsInRole("Admin"))
            throw new ForbiddenAccessException();

        if (request == null)
        {
            throw new ArgumentException($"{nameof(request)} es requerido");
        }

        if (request.BusinessId == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(request.BusinessId)} es requerido");
        }

        var project = new Project
        {
            BusinessId = request.BusinessId,
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        return new ProjectResponse
        {
            BusinessId = project.BusinessId,
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            CreatedAt = project.CreatedAt
        };
    }


    public async Task<ProjectResponse> UpdateAsync(Guid id, UpdateProjectRequest request, CancellationToken cancellationToken)
    {

        if (request == null)
        {
            throw new ArgumentException($"{nameof(request)} es requerido");
        }

        if (request.BusinessId == Guid.Empty)
        {
            throw new ArgumentException($"{nameof(request.BusinessId)} es requerido");
        }

        var project = await _context.Projects.FindAsync(request.Id);

        if (project == null)
            throw new KeyNotFoundException($"Proyecto con Id {request.Id} no encontrado.");

        // Actualizar campos
        project.Name = request.Name;
        project.Description = request.Description;
        project.StartDate = request.StartDate;
        project.EndDate = request.EndDate;

        await _context.SaveChangesAsync(cancellationToken);

        return new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            BusinessId = project.BusinessId,
            CreatedAt = project.CreatedAt
        };
    }


    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
