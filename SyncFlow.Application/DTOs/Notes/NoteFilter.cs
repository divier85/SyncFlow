using SyncFlow.Application.DTOs.Shared;
using System;

namespace SyncFlow.Application.DTOs.Notes
{
    public class NoteFilter: PagedFilterDto
    {
        public Guid taskId { get; set; }
    }
}
