// Base entity for domain models
using System;
using System.ComponentModel.DataAnnotations;

namespace SyncFlow.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public Guid CreatedById { get; private set; }
        public Guid? ModifiedById { get; private set; }
        public Guid? DeletedById { get; private set; }


        public bool IsDeleted => DeletedAt.HasValue;

        /* Métodos de dominio */
        public void MarkCreated(Guid userId)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedById = userId;
        }

        public void MarkUpdated(Guid userId)
        {
            UpdatedAt = DateTime.UtcNow;
            ModifiedById = userId;
        }

        public void MarkDeleted(Guid userId)
        {
            DeletedAt = DateTime.UtcNow;
            DeletedById = userId;
        }
    }
}