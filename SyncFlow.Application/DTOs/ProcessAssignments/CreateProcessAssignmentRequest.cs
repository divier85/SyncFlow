using System;

namespace SyncFlow.Application.DTOs.ProcessAssignments
{
    public class CreateProcessAssignmentRequest
    {
        public Guid ProcessId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
    }
}
