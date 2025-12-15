using System;

namespace SyncFlow.Application.DTOs.Shared
{
    public class FilterParams
    {
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
