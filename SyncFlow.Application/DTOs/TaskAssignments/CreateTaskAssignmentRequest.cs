using System;

namespace SyncFlow.Application.DTOs.TaskAssignments
{
    public class CreateTaskAssignmentRequest
    {
        public Guid TaskId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
    }
}
