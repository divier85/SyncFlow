using System;

namespace SyncFlow.Application.DTOs.Phases
{
    public class CreatePhaseRequest
    {
        public string Title { get; set; } = string.Empty;
        public Guid ProcessId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
