using Microsoft.AspNetCore.Identity;
using System;

namespace SyncFlow.Persistence.Auth
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public virtual ApplicationUser User { get; set; } = default!;
        public virtual ApplicationRole Role { get; set; } = default!;
    }
}
