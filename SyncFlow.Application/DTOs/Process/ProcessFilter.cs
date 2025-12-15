using SyncFlow.Application.DTOs.Shared;
using System;

namespace SyncFlow.Application.DTOs.Process
{
    public class ProcessFilter: PagedFilterDto
    {
        public Guid? ProjectId { get; set; } 
        public string? Name { get; set; }
    }
}
