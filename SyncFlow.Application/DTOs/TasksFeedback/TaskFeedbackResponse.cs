using System;

namespace SyncFlow.Application.DTOs.TasksFeedback
{
    public class TaskFeedbackResponse
    {
        public Guid Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

}
