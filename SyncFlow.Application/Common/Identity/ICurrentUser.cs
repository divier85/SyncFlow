using System;
using System.Collections.Generic;

namespace SyncFlow.Application.Common.Identity;

public interface ICurrentUser
{
    Guid UserId { get; }
    Guid BusinessId { get; }
    bool IsInRole(string role);
    IReadOnlyCollection<string> Roles { get; }
}
