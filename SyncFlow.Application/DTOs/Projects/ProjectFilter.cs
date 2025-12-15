using SyncFlow.Application.DTOs.Shared;
using System;

namespace SyncFlow.Application.DTOs.Projects
{
    public class ProjectFilter : PagedFilterDto
    {
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
