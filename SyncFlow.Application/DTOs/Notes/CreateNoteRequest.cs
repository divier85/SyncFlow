using System;

namespace SyncFlow.Application.DTOs.Notes
{
    public class CreateNoteRequest
    {
        public string Content { get; set; } = string.Empty;
        public Guid TaskId { get; set; }
    }

}
