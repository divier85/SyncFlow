using SyncFlow.Application.DTOs.TaskAssignments;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities = SyncFlow.Domain.Entities;

namespace SyncFlow.Infrastructure.Services
{
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly ISyncFlowDbContext _context;

        public TaskAssignmentService(ISyncFlowDbContext context)
        {
            _context = context;
        }

        public async Task<TaskAssignmentResponse> AssignAsync(CreateTaskAssignmentRequest request, CancellationToken cancellationToken)
        {
            var assignment = new Entities.TaskAssignment
            {
                TaskId = request.TaskId,
                UserId = request.UserId,
                RoleId = request.RoleId,
                AssignedAt = DateTime.UtcNow
            };

            await _context.TaskAssignments.AddAsync(assignment);
            await _context.SaveChangesAsync(cancellationToken);

            return new TaskAssignmentResponse
            {
                Id = assignment.Id,
                TaskId = assignment.TaskId,
                UserId = assignment.UserId,
                RoleId = assignment.RoleId,
                AssignedAt = assignment.AssignedAt
            };
        }

        public async Task<TaskAssignmentResponse> UpdateAssignAsync(Guid id, UpdateTaskAssignmentRequest request, CancellationToken cancellationToken)
        {
            var assignment = await _context.TaskAssignments.FindAsync(request.Id);

            if (assignment == null)
                throw new KeyNotFoundException($"Assignment con Id {request.Id} no encontrado.");


            assignment.TaskId = request.TaskId;
            assignment.UserId = request.UserId;
            assignment.RoleId = request.RoleId;
            assignment.AssignedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new TaskAssignmentResponse
            {
                Id = assignment.Id,
                TaskId = assignment.TaskId,
                UserId = assignment.UserId,
                RoleId = assignment.RoleId,
                AssignedAt = assignment.AssignedAt
            };
        }

        public async Task<List<TaskAssignmentResponse>> GetByTaskIdAsync(Guid taskId)
        {
            var results = _context.TaskAssignments
                .Where(x => x.TaskId == taskId)
                .Select(a => new TaskAssignmentResponse
                {
                    Id = a.Id,
                    TaskId = a.TaskId,
                    UserId = a.UserId,
                    RoleId = a.RoleId,
                    AssignedAt = a.AssignedAt
                });

            return await Task.FromResult(results.ToList());
        }
    }
}
