using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyncFlow.Application.Common.Tenant;
using SyncFlow.Application.DTOs.Tasks;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities = SyncFlow.Domain.Entities;

[Authorize(Roles = "Admin")]
[ApiController, Route("api/task-statuses")]
public class TaskStatusesController : ControllerBase
{
    private readonly ISyncFlowDbContext _db;
    private readonly IBusinessContext _biz;
    public TaskStatusesController(ISyncFlowDbContext db, IBusinessContext biz)
        => (_db, _biz) = (db, biz);

    [HttpGet]
    public async Task<IEnumerable<TaskStatusDto>> Get()
        => await _db.TaskStatuses
              .Where(s => s.BusinessId == _biz.BusinessId)
              .Select(s => new TaskStatusDto(s.Id, s.Name, s.Core, s.UIColor))
              .ToListAsync();

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskStatusDto dto)
    {
        var entity = new Entities.TaskStatus
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Core = dto.Core,
            UIColor = dto.UIColor,
            BusinessId = _biz.BusinessId
        };
        _db.TaskStatuses.Add(entity);
        await _db.SaveChangesAsync(CancellationToken.None);
        return CreatedAtAction(nameof(Get), new { id = entity.Id },
            new TaskStatusDto(entity.Id, entity.Name, entity.Core, entity.UIColor));
    }
}
