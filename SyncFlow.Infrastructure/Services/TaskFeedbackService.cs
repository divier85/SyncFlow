using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.DTOs.TasksFeedback;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Domain.Entities;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Services
{
    public class TaskFeedbackService : ITaskFeedbackService
    {
        private readonly ISyncFlowDbContext _context;

        public TaskFeedbackService(ISyncFlowDbContext context)
        {
            _context = context;
        }

        public async Task<TaskFeedbackResponse> CreateAsync(CreateTaskFeedbackRequest request, CancellationToken cancellationToken)
        {
            var feedback = new TaskFeedback
            {
                Comment = request.Comment,
                TaskId = request.TaskId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.TaskFeedbacks.AddAsync(feedback, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new TaskFeedbackResponse
            {
                Id = feedback.Id,
                Comment = feedback.Comment,
                CreatedAt = feedback.CreatedAt
            };
        }

        public async Task<List<TaskFeedbackResponse>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken)
        {
            return await _context.TaskFeedbacks
                .Where(f => f.TaskId == taskId)
                .Select(f => new TaskFeedbackResponse
                {
                    Id = f.Id,
                    Comment = f.Comment,
                    CreatedAt = f.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }

}
