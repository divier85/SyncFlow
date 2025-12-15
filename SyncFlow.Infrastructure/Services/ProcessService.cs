using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.Common.Models;
using SyncFlow.Application.DTOs.Process;
using SyncFlow.Application.DTOs.ProcessAssignments;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Domain.Entities;
using SyncFlow.Infrastructure.Common;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services
{
    public class ProcessService : IProcessService
    {
        private readonly ISyncFlowDbContext _context;
        public ProcessService(ISyncFlowDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ProcessResponse>> GetAllAsync(ProcessFilter filter)
        {
            var query = _context.Processes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(p => p.Name.Contains(filter.Name));

            if (filter.ProjectId.HasValue)
                query = query.Where(p => p.ProjectId == filter.ProjectId);

            var processes = query.Select(p => new ProcessResponse
            {
                Id = p.Id,
                Name = p.Name,
                ProjectId = p.ProjectId,
                Phases = p.Phases.Select(ph => new PhaseSummary()
                {
                    Id = ph.Id,
                    Title = ph.Title,
                    ProcessId = ph.ProcessId,
                    Tasks = ph.Tasks.Select(t => new TaskSummary()
                    {
                        AssignedToId = t.AssignedToId,
                        Description = t.Description,
                        DueDate = t.DueDate,
                        Id = t.Id,
                        PhaseId = t.PhaseId,
                        Title = t.Title
                    }).ToList()
                }).ToList()
            });

            return await processes.ToPagedResultAsync(filter);
        }

        public async Task<ProcessResponse?> GetByIdAsync(Guid id)
        {
            var p = await _context.Processes.FindAsync(id);
            if (p == null) return null;

            return new ProcessResponse
            {
                Id = p.Id,
                Name = p.Name,
                ProjectId = p.Id,
                Phases = p.Phases.Select(ph => new PhaseSummary()
                {
                    Id = ph.Id,
                    Title = ph.Title,
                    ProcessId = ph.ProcessId,
                    Tasks = ph.Tasks.Select(t => new TaskSummary()
                    {
                        AssignedToId = t.AssignedToId,
                        Description = t.Description,
                        DueDate = t.DueDate,
                        Id = t.Id,
                        PhaseId = t.PhaseId,
                        Title = t.Title
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<ProcessResponse> CreateAsync(CreateProcessRequest request, CancellationToken cancellationToken)
        {
            var process = new Process
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                ProjectId = request.ProjectId,
                Phases = request.Phases.Select(p => new Phase()
                {
                    Title = p.Title,
                    Id = Guid.NewGuid(),
                    Tasks = p.Tasks.Select(t => new Domain.Entities.Task()
                    {
                        Description = t.Description,
                        DueDate = t.DueDate,
                        Id = Guid.NewGuid(),
                        Title = t.Title,
                    }).ToList()
                }).ToList()
            };

            _context.Processes.Add(process);
            await _context.SaveChangesAsync(cancellationToken);

            return new ProcessResponse
            {
                Id = process.Id,
                Name = process.Name,
                ProjectId = process.Id,
                Phases = process.Phases.Select(ph => new PhaseSummary()
                {
                    Id = ph.Id,
                    Title = ph.Title,
                    ProcessId = ph.ProcessId,
                    Tasks = ph.Tasks.Select(t => new TaskSummary()
                    {
                        AssignedToId = t.AssignedToId,
                        Description = t.Description,
                        DueDate = t.DueDate,
                        Id = t.Id,
                        PhaseId = t.PhaseId,
                        Title = t.Title
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<ProcessResponse> UpdateAsync(Guid id, UpdateProcessRequest request, CancellationToken cancellationToken)
        {
            var process = await _context.Processes.FindAsync(request.Id);

            if (process == null)
                throw new KeyNotFoundException($"Proceso con Id {request.Id} no encontrado.");

            // Actualizar campos
            process.Name = request.Name;
            process.ProjectId = request.ProjectId;


            await _context.SaveChangesAsync(cancellationToken);

            return new ProcessResponse
            {
                Id = process.Id,
                Name = process.Name,
                ProjectId = process.Id,
                Phases = process.Phases.Select(ph => new PhaseSummary()
                {
                    Id = ph.Id,
                    Title = ph.Title,
                    ProcessId = ph.ProcessId,
                    Tasks = ph.Tasks.Select(t => new TaskSummary()
                    {
                        AssignedToId = t.AssignedToId,
                        Description = t.Description,
                        DueDate = t.DueDate,
                        Id = t.Id,
                        PhaseId = t.PhaseId,
                        Title = t.Title
                    }).ToList()
                }).ToList()
            };
        }


        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var project = await _context.Processes.FindAsync(id);
            if (project == null) return false;

            _context.Processes.Remove(project);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<ProcessAssignmentResponse> AssignProcessAsync(CreateProcessAssignmentRequest request, CancellationToken cancellationToken)
        {
            if (request.UserId is null && request.RoleId is null)
                throw new ArgumentException("Debe asignarse a un usuario o a un rol.");

            var assignment = new ProcessAssignment
            {
                Id = Guid.NewGuid(),
                ProcessId = request.ProcessId,
                UserId = request.UserId,
                RoleId = request.RoleId
            };

            _context.ProcessAssignments.Add(assignment);
            await _context.SaveChangesAsync(cancellationToken);

            return new ProcessAssignmentResponse
            {
                Id = assignment.Id,
                ProcessId = assignment.ProcessId,
                UserId = assignment.UserId,
                RoleId = assignment.RoleId
            };
        }

        public async Task<IEnumerable<ProcessResponse>> GetProcessesByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.ProcessAssignments
                .Where(pa => pa.UserId == userId)
                .Select(pa => new ProcessResponse
                {
                    Id = pa.Process.Id,
                    Name = pa.Process.Name,
                    ProjectId = pa.Process.ProjectId
                }).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProcessResponse>> GetProcessesByRoleAsync(Guid roleId, CancellationToken cancellationToken)
        {
            return await _context.ProcessAssignments
                .Where(pa => pa.RoleId == roleId)
                .Select(pa => new ProcessResponse
                {
                    Id = pa.Process.Id,
                    Name = pa.Process.Name,
                    ProjectId = pa.Process.ProjectId
                }).ToListAsync(cancellationToken);
        }

        public async Task<ProcessAssignmentResponse> UpdateProcessAssignmentAsync(Guid assignmentId, CreateProcessAssignmentRequest request, CancellationToken cancellationToken)
        {
            var assignment = await _context.ProcessAssignments.FindAsync(new object[] { assignmentId }, cancellationToken);
            if (assignment == null) throw new KeyNotFoundException("Asignación no encontrada");

            assignment.UserId = request.UserId;
            assignment.RoleId = request.RoleId;

            await _context.SaveChangesAsync(cancellationToken);

            return new ProcessAssignmentResponse
            {
                Id = assignment.Id,
                ProcessId = assignment.ProcessId,
                UserId = assignment.UserId,
                RoleId = assignment.RoleId
            };
        }

        public async Task<bool> RemoveProcessAssignmentAsync(Guid assignmentId, CancellationToken cancellationToken)
        {
            var assignment = await _context.ProcessAssignments.FindAsync(new object[] { assignmentId }, cancellationToken);
            if (assignment == null) return false;

            _context.ProcessAssignments.Remove(assignment);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }


    }
}
