using System;
using System.Collections.Generic;

namespace SyncFlow.Application.DTOs.Dashboard
{
    public record DashboardSummary(
     int TotalProjects,
     int TotalProcesses,
     int TotalTasks,
     int TasksPending,
     int TasksDueSoon,          // <= 3 días
     int TasksOverdue,
     int TasksCompletedToday,
     IEnumerable<UserLoad> LoadByUser);

    public record UserLoad(Guid UserId, string UserName, int ActiveTasks);

}
