using System;
using System.ComponentModel.DataAnnotations;

namespace SyncFlow.Application.DTOs.Process
{
    public class UpdateProcessRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public Guid ProjectId { get; set; }
    }
}
