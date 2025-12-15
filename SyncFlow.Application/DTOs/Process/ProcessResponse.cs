using SyncFlow.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SyncFlow.Application.DTOs.Process
{
    public class ProcessResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProjectId { get; set; }

        // Opcional: podrías incluir un resumen de las fases si deseas exponerlas
        public List<PhaseSummary>? Phases { get; set; }
    }

    public class PhaseSummary
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public Guid ProcessId { get; set; }
    
        public List<TaskSummary> Tasks { get; set; }
    }

    public class TaskSummary {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }

        public Guid PhaseId { get; set; }

        public Guid? AssignedToId { get; set; }
        //public User? AssignedTo { get; set; }
    }
}
