using System;

namespace SyncFlow.Application.DTOs.TaskAssignments
{
    public class TaskAssignmentResponse
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
        public DateTime AssignedAt { get; set; }
    }
}
