using System;

namespace SyncFlow.Application.DTOs.ProcessAssignments
{
    public class ProcessAssignmentResponse
    {
        public Guid Id { get; set; }
        public Guid ProcessId { get; set; }
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
    }
}
