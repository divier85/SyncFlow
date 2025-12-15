using System;

namespace SyncFlow.Application.DTOs.Projects
{
    public class ProjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid BusinessId { get; set; }
    }
}
