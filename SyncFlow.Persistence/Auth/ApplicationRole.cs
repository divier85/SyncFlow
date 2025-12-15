using Microsoft.AspNetCore.Identity;
using SyncFlow.Domain.Common;
using SyncFlow.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SyncFlow.Persistence.Auth;

public class ApplicationRole : IdentityRole<Guid>, IMultiTenantEntity, IRole
{
    public Guid BusinessId { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

    public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    public ICollection<ProcessAssignment> ProcessAssignments { get; set; } = new List<ProcessAssignment>();

}
