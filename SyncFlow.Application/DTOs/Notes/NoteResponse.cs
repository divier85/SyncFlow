using System;

namespace SyncFlow.Application.DTOs.Notes
{
    public class NoteResponse
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

}
