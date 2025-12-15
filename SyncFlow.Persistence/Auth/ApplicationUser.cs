using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SyncFlow.Persistence.Auth;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid BusinessId { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

}
