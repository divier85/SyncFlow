using System;

namespace SyncFlow.Application.DTOs.Tasks;

public class UpdateTaskRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid StatusId { get; set; }   // requerido
    public Guid PhaseId { get; set; }
    public Guid? AssignedToId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
