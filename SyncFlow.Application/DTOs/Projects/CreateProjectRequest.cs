using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SyncFlow.Application.DTOs.Projects;

public class CreateProjectRequest : IValidatableObject
{
    [Required]
    public Guid BusinessId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate < StartDate)
        {
            yield return new ValidationResult(
                "La fecha de finalización no puede ser menor que la fecha de inicio.",
                new[] { nameof(EndDate) }
            );
        }
    }
}
