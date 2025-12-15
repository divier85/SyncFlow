using System;

namespace SyncFlow.Application.DTOs.TasksFeedback
{
    public class CreateTaskFeedbackRequest
    {
        public string Comment { get; set; } = string.Empty;
        public Guid TaskId { get; set; }
    }

}
