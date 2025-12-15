using SyncFlow.Application.DTOs.Shared;
using System;

namespace SyncFlow.Application.DTOs.Tasks
{
    public class TaskFilter : PagedFilterDto
    {
        public string? Title { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
    }
}
