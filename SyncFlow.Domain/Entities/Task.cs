using SyncFlow.Domain.Common;
using System;
using System.Collections.Generic;

namespace SyncFlow.Domain.Entities;

public class Task : BaseEntity, IMultiTenantEntity
{
    public Guid BusinessId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }

    public Guid PhaseId { get; set; }
    public Phase Phase { get; set; } = null!;

    public Guid? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }
    public Guid StatusId { get; set; }
    public TaskStatus Status { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public ICollection<TaskFeedback> Feedbacks { get; set; } = new List<TaskFeedback>();
    public ICollection<Note> Notes { get; set; } = new List<Note>();
}
