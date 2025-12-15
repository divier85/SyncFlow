using SyncFlow.Application.DTOs.TasksFeedback;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Application.Interfaces.Services
{
    public interface ITaskFeedbackService
    {
        Task<TaskFeedbackResponse> CreateAsync(CreateTaskFeedbackRequest request, CancellationToken cancellationToken);
        Task<List<TaskFeedbackResponse>> GetByTaskIdAsync(Guid taskId, CancellationToken cancellationToken);
    }

}
