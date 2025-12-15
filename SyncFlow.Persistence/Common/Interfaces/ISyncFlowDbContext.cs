using Microsoft.EntityFrameworkCore;
using SyncFlow.Domain.Entities;
using SyncFlow.Persistence.Auth;
using SyncFlow.Persistence.Contexts;
using System.Threading;
using System.Threading.Tasks;

namespace SyncFlow.Persistence.Common.Interfaces;

public interface ISyncFlowDbContext
{
    DbSet<ApplicationRole> Roles { get; }
    DbSet<ApplicationUserRole> UserRoles { get; }
    DbSet<Business> Businesses { get; }
    DbSet<Project> Projects { get; }
    DbSet<Process> Processes { get; }
    DbSet<Phase> Phases { get; }
    DbSet<Domain.Entities.Task> Tasks { get; }
    DbSet<TaskFeedback> TaskFeedbacks { get; }
    DbSet<Note> Notes { get; }
    DbSet<TaskAssignment> TaskAssignments { get; }
    DbSet<ProcessAssignment> ProcessAssignments { get; }
    DbSet<User> Users { get; }

    DbSet<Domain.Entities.TaskStatus> TaskStatuses { get; }

    DbSet<ProcessTemplate> ProcessTemplates { get; }

    DbSet<TaskTemplate> TaskTemplates { get; }

    DbSet<PhaseTemplate> PhaseTemplates { get; }

    DbSet<AuditLogEntry> AuditLogEntries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    System.Threading.Tasks.Task SeedCoreStatusesAsync(SyncFlowDbContext db);
}

