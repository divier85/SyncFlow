using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyncFlow.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncFlow.Infrastructure.Common.Identity;

public static class UserManagerExtensions
{
    public static async Task<IList<string>> GetRolesIgnoreFiltersAsync<TUser>(
        this UserManager<TUser> userManager,
        TUser user,
        SyncFlowDbContext db) where TUser : class
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var userId = await userManager.GetUserIdAsync(user);

        var roleNames = await db.UserRoles
            .Where(ur => ur.UserId == Guid.Parse(userId))
            .Join(
                db.Roles.IgnoreQueryFilters(),
                ur => ur.RoleId,
                r => r.Id,
                (ur, r) => r.Name
            )
            .ToListAsync();

        return roleNames;
    }
}
