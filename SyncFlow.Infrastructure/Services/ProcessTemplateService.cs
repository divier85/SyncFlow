using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using SyncFlow.Application.Common.Identity;
using SyncFlow.Application.Common.Notifications;
using SyncFlow.Application.Common.Tenant;
using SyncFlow.Application.DTOs.AuditLogs;
using SyncFlow.Application.DTOs.Process;
using SyncFlow.Application.DTOs.Projects;
using SyncFlow.Application.DTOs.Templates;
using SyncFlow.Application.Interfaces.Services;
using SyncFlow.Domain.Entities;
using SyncFlow.Persistence.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace SyncFlow.Infrastructure.Services
{
    public class ProcessTemplateService : IProcessTemplateService
    {
        private readonly ISyncFlowDbContext _db;
        private readonly IBusinessContext _biz;
        private readonly INotificationService _notificationService;
        private readonly ICurrentUser _current;
        private readonly IAuditService _auditService;

        public ProcessTemplateService(ISyncFlowDbContext db, IBusinessContext biz, INotificationService notificationService, ICurrentUser current, IAuditService auditService)
            => (_db, _biz, _notificationService, _current, _auditService) = (db, biz, notificationService, current, auditService);

        public async Task<List<ProcessTemplateDto>> GetAllAsync(CancellationToken ct)
        {
            return await _db.ProcessTemplates
                .Where(p => p.BusinessId == _biz.BusinessId)
                .Include(p => p.Phases)
                    .ThenInclude(ph => ph.Tasks)
                .Select(p => new ProcessTemplateDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.IsDraft,
                    p.Phases.Select(ph => new PhaseTemplateDto(
                        ph.Id,
                        ph.Name,
                        ph.Description,
                        ph.Order,
                        ph.Tasks.Select(t => new TaskTemplateDto(t.Id, t.Title, t.Description, t.EstimatedHours)).ToList()
                    )).ToList()
                ))
                .ToListAsync(ct);
        }

        public async Task<List<ProcessTemplateDto>> GetLatestTemplatesAsync(CancellationToken ct)
        {
            var latestTemplates = await _db.ProcessTemplates
                .Where(p => p.IsActive && !p.IsDeleted)
                .GroupBy(p => p.Name)
                .Select(g => g.OrderByDescending(t => t.Version).First())
                .Include(p => p.Phases)
                    .ThenInclude(ph => ph.Tasks)
                .ToListAsync(ct);

            return latestTemplates.Select(template => new ProcessTemplateDto(
                template.Id,
                template.Name,
                template.Description,
                template.IsDraft,
                template.Phases.Select(ph => new PhaseTemplateDto(
                    ph.Id,
                    ph.Name,
                    ph.Description,
                    ph.Order,
                    ph.Tasks.Select(t => new TaskTemplateDto(t.Id, t.Title, t.Description, t.EstimatedHours)).ToList()
                )).ToList()
            )).ToList();
        }

        public async Task<List<ProcessTemplateDto>> GetVersionsByNameAsync(string name)
        {
            var templates = await _db.ProcessTemplates
                .Where(p => p.Name == name && !p.IsDeleted)
                .OrderByDescending(p => p.Version)
                .ToListAsync();

            return templates.Select(template => new ProcessTemplateDto(
                template.Id,
                template.Name,
                template.Description,
                template.IsDraft,
                template.Phases.Select(ph => new PhaseTemplateDto(
                    ph.Id,
                    ph.Name,
                    ph.Description,
                    ph.Order,
                    ph.Tasks.Select(t => new TaskTemplateDto(t.Id, t.Title, t.Description, t.EstimatedHours)).ToList()
                )).ToList()
            )).ToList();
        }

        public async Task<ProcessTemplateDto?> GetByIdAsync(Guid id, CancellationToken ct)
        {

            var p = await _db.ProcessTemplates.FindAsync(id);
            if (p == null) return null;

            return new ProcessTemplateDto(
                    p.Id,
                    p.Name,
                    p.Description,
                    p.IsDraft,
                    p.Phases.Select(ph => new PhaseTemplateDto(
                        ph.Id,
                        ph.Name,
                        ph.Description,
                        ph.Order,
                        ph.Tasks.Select(t => new TaskTemplateDto(t.Id, t.Title, t.Description, t.EstimatedHours)).ToList()
                    )).ToList()
                    );

        }

        public async Task<Guid> CreateAsync(ProcessTemplateDto dto, CancellationToken ct)
        {
            var latestVersion = await _db.ProcessTemplates
                .Where(p => p.Name == dto.Name)
                .MaxAsync(p => (int?)p.Version, ct) ?? 0;

            var processTemplate = new ProcessTemplate
            {
                Name = dto.Name,
                Description = dto.Description,
                Version = latestVersion + 1,
                IsActive = true,
                IsDraft = dto.IsDraft,
                Phases = dto.Phases.Select(ph => new PhaseTemplate
                {
                    Name = ph.Name,
                    Order = ph.Order,
                    Tasks = ph.Tasks.Select(t => new TaskTemplate
                    {
                        Description = t.Description,
                        EstimatedHours = t.EstimatedHours,
                        Title = t.Title,

                    }).ToList()
                }).ToList()
            };

            _db.ProcessTemplates.Add(processTemplate);
            await _db.SaveChangesAsync(ct);

            return processTemplate.Id;
        }

        public async Task PublishAsync(Guid id, CancellationToken ct)
        {
            var template = await _db.ProcessTemplates
                .Include(p => p.Phases)
                    .ThenInclude(ph => ph.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (template == null)
                throw new KeyNotFoundException($"Template con ID {id} no encontrado.");

            if (!template.IsDraft)
                throw new InvalidOperationException("La plantilla ya fue publicada.");

            // ✅ Validar fases y tareas
            if (template.Phases == null || !template.Phases.Any())
                throw new InvalidOperationException("La plantilla no tiene fases definidas.");

            if (template.Phases.Any(phase => phase.Tasks == null || !phase.Tasks.Any()))
                throw new InvalidOperationException("Todas las fases deben tener al menos una tarea.");

            // 🔄 Desactivar versiones anteriores activas
            var activeVersions = await _db.ProcessTemplates
                .Where(p => p.Name == template.Name && p.IsActive && p.Id != id)
                .ToListAsync();

            foreach (var version in activeVersions)
                version.IsActive = false;

            // 🟢 Publicar versión actual
            template.IsDraft = false;
            template.IsActive = true;

            // 📝 Auditoría usando servicio
            await _auditService.RegisterAsync(new AuditLogDto
            {
                Action = "PublishTemplate",
                EntityId = template.Id,
                EntityType = "ProcessTemplate",
                PerformedBy = _current.UserId,
                PerformedAt = DateTime.UtcNow,
                Details = $"Publicación de la versión {template.Version} de la plantilla '{template.Name}'."
            }, ct);

            // ✉️ Notificación
            await _notificationService.SendAsync(new NotificationMessage(
                                _biz.BusinessId,
                                new[] { _current.UserId },
                                "Plantilla publicada",
                                $"<p> La plantilla '{template.Name}' versión {template.Version} fue publicada exitosamente.</p>"
                            ), ct);

            await _db.SaveChangesAsync(ct);
        }

        public async Task<ProcessTemplateDto> UpdateAsync(Guid id, ProcessTemplateDto request, CancellationToken cancellationToken)
        {
            var template = await _db.ProcessTemplates
                .Include(t => t.Phases)
                    .ThenInclude(p => p.Tasks)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (template == null)
                throw new KeyNotFoundException($"Template con Id {request.Id} no encontrado.");

            // 🔒 Validación para evitar modificar una plantilla no editable
            if (!template.IsDraft)
                throw new InvalidOperationException("No se puede modificar una plantilla que no está en estado de borrador.");

            // ✅ Actualizar campos base
            template.Description = request.Description;
            template.Name = request.Name;

            foreach (var phaseDto in request.Phases)
            {
                var existingPhase = template.Phases.FirstOrDefault(p => p.Id == phaseDto.Id);

                if (existingPhase != null)
                {
                    existingPhase.Name = phaseDto.Name;
                    existingPhase.Order = phaseDto.Order;

                    foreach (var taskDto in phaseDto.Tasks)
                    {
                        var existingTask = existingPhase.Tasks.FirstOrDefault(t => t.Id == taskDto.Id);

                        if (existingTask != null)
                        {
                            existingTask.Title = taskDto.Title;
                            existingTask.Description = taskDto.Description;
                            existingTask.EstimatedHours = taskDto.EstimatedHours;
                        }
                        else
                        {
                            existingPhase.Tasks.Add(new TaskTemplate
                            {
                                Id = Guid.NewGuid(),
                                Title = taskDto.Title,
                                Description = taskDto.Description,
                                EstimatedHours = taskDto.EstimatedHours
                            });
                        }
                    }

                    existingPhase.Tasks.RemoveAll(t => !phaseDto.Tasks.Any(dto => dto.Id == t.Id));
                }
                else
                {
                    template.Phases.Add(new PhaseTemplate
                    {
                        Id = Guid.NewGuid(),
                        Name = phaseDto.Name,
                        Order = phaseDto.Order,
                        Tasks = phaseDto.Tasks.Select(t => new TaskTemplate
                        {
                            Id = Guid.NewGuid(),
                            Title = t.Title,
                            Description = t.Description,
                            EstimatedHours = t.EstimatedHours
                        }).ToList()
                    });
                }
            }

            template.Phases.RemoveAll(p => !request.Phases.Any(dto => dto.Id == p.Id));

            await _db.SaveChangesAsync(cancellationToken);

            return new ProcessTemplateDto(
                template.Id,
                template.Name,
                template.Description,
                template.IsDraft,
                template.Phases.Select(ph => new PhaseTemplateDto(
                    ph.Id,
                    ph.Name,
                    ph.Description,
                    ph.Order,
                    ph.Tasks.Select(t => new TaskTemplateDto(t.Id, t.Title, t.Description, t.EstimatedHours)).ToList()
                )).ToList()
            );
        }



        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {

            var template = await _db.ProcessTemplates.FindAsync(id);
            if (template == null) return false;

            _db.ProcessTemplates.Remove(template);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }

}
