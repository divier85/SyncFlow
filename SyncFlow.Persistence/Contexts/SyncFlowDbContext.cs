using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SyncFlow.Application.Common.Identity;
using SyncFlow.Application.Common.Tenant;
using SyncFlow.Domain.Common;
using SyncFlow.Domain.Entities;
using SyncFlow.Domain.Enums;
using SyncFlow.Persistence.Auth;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Entities = SyncFlow.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace SyncFlow.Persistence.Contexts;

public class SyncFlowDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, ISyncFlowDbContext
{
    private readonly IBusinessContext _biz;
    private readonly ICurrentUser _currentUser;
    public SyncFlowDbContext(DbContextOptions<SyncFlowDbContext> options, IBusinessContext biz, ICurrentUser currentUser)
        : base(options)
    {
        _biz = biz;
        _currentUser = currentUser;
    }

    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ApplicationRole> Roles => Set<ApplicationRole>();
    public DbSet<ApplicationUserRole> UserRoles => Set<ApplicationUserRole>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Process> Processes => Set<Process>();
    public DbSet<Phase> Phases => Set<Phase>();
    public DbSet<Entities.Task> Tasks => Set<Entities.Task>();
    public DbSet<TaskFeedback> TaskFeedbacks => Set<TaskFeedback>();
    public DbSet<Note> Notes => Set<Note>();

    public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();
    public DbSet<ProcessAssignment> ProcessAssignments => Set<ProcessAssignment>();

    public DbSet<Entities.TaskStatus> TaskStatuses => Set<Entities.TaskStatus>();

    public DbSet<ProcessTemplate> ProcessTemplates => Set<ProcessTemplate>();

    public DbSet<TaskTemplate> TaskTemplates => Set<TaskTemplate>();

    public DbSet<PhaseTemplate> PhaseTemplates => Set<PhaseTemplate>();

    public DbSet<AuditLogEntry> AuditLogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var et in modelBuilder.Model.GetEntityTypes()
                   .Where(t => typeof(IMultiTenantEntity).IsAssignableFrom(t.ClrType)))
        {
            // Solo aplica si la entidad tiene "BusinessId"
            if (et.FindProperty("BusinessId") is null) continue;

            var param = Expression.Parameter(et.ClrType, "e");

            //
            // 1) FILTRO DE SOFT-DELETE (solo si existe DeletedAt)
            //
            Expression isNotDeleted = Expression.Constant(true);
            if (et.FindProperty("DeletedAt") is not null)
            {
                var deletedProp = Expression.Property(param, "DeletedAt");
                isNotDeleted = Expression.Equal(deletedProp, Expression.Constant(null));
            }

            //
            // 2) FILTRO DE TENANT
            //
            var bizCtxProp = Expression.Property(
                Expression.Constant(_biz),
                nameof(IBusinessContext.BusinessId));

            var entityBiz = Expression.Property(param, "BusinessId");
            var sameTenant = Expression.Equal(entityBiz, bizCtxProp);

            // Si BusinessId == Guid.Empty => omitir filtro tenant
            var guidEmpty = Expression.Equal(bizCtxProp, Expression.Constant(Guid.Empty));
            var finalExpr = Expression.AndAlso(
                                 isNotDeleted,
                                 Expression.OrElse(guidEmpty, sameTenant));

            var lambda = Expression.Lambda(finalExpr, param);
            modelBuilder.Entity(et.ClrType).HasQueryFilter(lambda);
        }


        foreach (var e in modelBuilder.Model.GetEntityTypes()
                              .Where(t => typeof(BaseEntity).IsAssignableFrom(t.ClrType)))
        {
            modelBuilder.Entity(e.ClrType)
              .HasQueryFilter(BuildIsDeletedFilter(e.ClrType));
        }

        // User-Role many-to-many
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Business)
            .WithMany(b => b.Users)
            .HasForeignKey(u => u.BusinessId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>().HasQueryFilter(p => p.BusinessId == null || p.BusinessId == _biz.BusinessId);


        modelBuilder.Entity<Role>()
            .HasOne(r => r.Business)
            .WithMany(b => b.Roles)
            .HasForeignKey(r => r.BusinessId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Role>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        modelBuilder.Entity<ApplicationRole>().HasIndex(r => r.NormalizedName).IsUnique(false);
        modelBuilder.Entity<ApplicationRole>().HasIndex(r => new { r.NormalizedName, r.BusinessId })
              .IsUnique();

        modelBuilder.Entity<ApplicationRole>()
            .HasQueryFilter(r => _biz.BusinessId == null || r.BusinessId == _biz.BusinessId);

        modelBuilder.Entity<ApplicationUserRole>(b =>
        {

            b.HasOne(x => x.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(x => x.UserId);

            b.HasOne(x => x.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Business)
            .WithMany(b => b.Projects)
            .HasForeignKey(p => p.BusinessId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        modelBuilder.Entity<Entities.TaskStatus>(e =>
        {
            e.HasIndex(t => new { t.BusinessId, t.Name }).IsUnique();
            e.Property(t => t.Core).HasConversion<byte>();
        });

        modelBuilder.Entity<Entities.TaskStatus>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        // User - Assigned Tasks one-to-many (nullable)
        modelBuilder.Entity<Entities.Task>()
            .HasOne(t => t.AssignedTo)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Entities.Task>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        modelBuilder.Entity<Entities.Task>()
           .HasOne(t => t.Status)
           .WithMany()
           .HasForeignKey(t => t.StatusId);

        // Project - Processes
        modelBuilder.Entity<Process>()
            .HasOne(p => p.Project)
            .WithMany(pj => pj.Processes)
            .HasForeignKey(p => p.ProjectId);

        modelBuilder.Entity<ProcessAssignment>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        modelBuilder.Entity<Process>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        // Process - Phases
        modelBuilder.Entity<Phase>()
            .HasOne(p => p.Process)
            .WithMany(pr => pr.Phases)
            .HasForeignKey(p => p.ProcessId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Phase>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        // Phase - Tasks
        modelBuilder.Entity<Entities.Task>()
            .HasOne(t => t.Phase)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.PhaseId)
             .OnDelete(DeleteBehavior.Cascade);

        // Task - Feedback
        modelBuilder.Entity<TaskFeedback>()
            .HasOne(f => f.Task)
            .WithMany(t => t.Feedbacks)
            .HasForeignKey(f => f.TaskId);

        modelBuilder.Entity<TaskFeedback>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);


        // Task - Notes
        modelBuilder.Entity<Note>()
            .HasOne(n => n.Task)
            .WithMany(t => t.Notes)
            .HasForeignKey(n => n.TaskId);

        modelBuilder.Entity<Note>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        modelBuilder.Entity<TaskTemplate>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        modelBuilder.Entity<ProcessTemplate>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        modelBuilder.Entity<PhaseTemplate>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);

        modelBuilder.Entity<AuditLogEntry>().HasQueryFilter(p => p.BusinessId == _biz.BusinessId);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.MarkCreated(_currentUser.UserId);
                    break;
                case EntityState.Modified:
                    entry.Entity.MarkUpdated(_currentUser.UserId);
                    break;
                case EntityState.Deleted:
                    // Soft delete → marcar IsDeleted y cambiar estado a Modified
                    entry.State = EntityState.Modified;
                    entry.Entity.MarkDeleted(_currentUser.UserId);
                    break;
            }
        }
        return base.SaveChangesAsync(ct);
    }

    public async Task SeedCoreStatusesAsync(SyncFlowDbContext db)
    {
        var businesses = await db.Businesses.ToListAsync();
        foreach (var biz in businesses)
        {
            if (db.TaskStatuses.IgnoreQueryFilters().FirstOrDefault(s => s.Name == "Pending" && s.BusinessId == biz.Id) == null)
            {
                db.TaskStatuses.Add(new Entities.TaskStatus { Name = "Pending", Core = CoreStatus.Pending, BusinessId = biz.Id });
            }

            if (db.TaskStatuses.IgnoreQueryFilters().FirstOrDefault(s => s.Name == "In Progress" && s.BusinessId == biz.Id) == null)
            {
                db.TaskStatuses.Add(new Entities.TaskStatus { Name = "In Progress", Core = CoreStatus.Active, BusinessId = biz.Id });
            }

            if (db.TaskStatuses.IgnoreQueryFilters().FirstOrDefault(s => s.Name == "Completed" && s.BusinessId == biz.Id) == null)
            {
                db.TaskStatuses.Add(new Entities.TaskStatus { Name = "Completed", Core = CoreStatus.Completed, BusinessId = biz.Id });
            }

            if (db.TaskStatuses.IgnoreQueryFilters().FirstOrDefault(s => s.Name == "Canceled" && s.BusinessId == biz.Id) == null)
            {
                db.TaskStatuses.Add(new Entities.TaskStatus { Name = "Canceled", Core = CoreStatus.Canceled, BusinessId = biz.Id });
            }

        }
        await db.SaveChangesAsync();
    }
    private static LambdaExpression BuildIsDeletedFilter(Type type)
    {
        var param = Expression.Parameter(type, "e");
        var prop = Expression.Property(param, nameof(BaseEntity.DeletedAt));
        var cond = Expression.Equal(prop, Expression.Constant(null));
        return Expression.Lambda(cond, param);
    }

    private static void SetGlobalQueryFilter<TEntity>(
       ModelBuilder builder, IBusinessContext biz) where TEntity : class, IMultiTenantEntity
       => builder.Entity<TEntity>().HasQueryFilter(e => e.BusinessId == biz.BusinessId);

}
