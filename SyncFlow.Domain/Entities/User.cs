using SyncFlow.Domain.Common;
using System;
using System.Collections.Generic;

namespace SyncFlow.Domain.Entities
{
    public class User : BaseEntity, IMultiTenantEntity
    {
        public string Identification { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = null;
        public string? Address { get; set; } = null;
        public string? City { get; set; } = null;
        public Guid BusinessId { get; set; }
        public Business Business { get; set; } = null!;

        public ICollection<Role> Roles { get; set; } = new List<Role>();
        public ICollection<Task> AssignedTasks { get; set; } = new List<Task>();

        public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
        public ICollection<ProcessAssignment> ProcessAssignments { get; set; } = new List<ProcessAssignment>();


    }
}