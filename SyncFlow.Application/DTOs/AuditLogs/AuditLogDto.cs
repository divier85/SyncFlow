using System;

namespace SyncFlow.Application.DTOs.AuditLogs
{
    public class AuditLogDto
    {
        public string Action { get; set; }
        public Guid EntityId { get; set; }
        public string EntityType { get; set; }
        public Guid PerformedBy { get; set; }
        public DateTime PerformedAt { get; set; }
        public string Details { get; set; }
    }

}
