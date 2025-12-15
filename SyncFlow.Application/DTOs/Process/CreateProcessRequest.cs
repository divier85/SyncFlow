using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SyncFlow.Application.DTOs.Process
{
    public class CreateProcessRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public Guid ProjectId { get; set; }

        public List<PhaseSummary>? Phases { get; set; }
    }
}
