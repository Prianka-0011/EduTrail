using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using EduTrail.Domain.Entities;
using  EduTrail.Domain.Interfaces;
using System.Text.Json;

namespace EduTrail.Infrastructure.Data
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly Guid _currentUserId;

        public AuditInterceptor(Guid currentUserId)
        {
            _currentUserId = currentUserId;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            AddAuditEntries(eventData.Context!);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            AddAuditEntries(eventData.Context!);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void AddAuditEntries(DbContext context)
        {
            var entries = context.ChangeTracker.Entries()
                .Where(e => e.Entity is IAuditable &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified)).ToList();

            foreach (var entry in entries)
            {
                var auditableEntity = (IAuditable)entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    auditableEntity.CreatedDate = now;
                    auditableEntity.CreatedById = _currentUserId;
                }
                else if (entry.State == EntityState.Modified)
                {
                    auditableEntity.UpdatedDate = now;
                    auditableEntity.UpdatedById = _currentUserId;
                }

                var recordIdProperty = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Id");
                if (recordIdProperty == null) continue;

                var recordId = recordIdProperty.CurrentValue is Guid guidValue ? guidValue : Guid.Empty;

                var modifiedProps = entry.Properties
                    .Where(p => p.IsModified)
                    .Select(p => p.Metadata.Name)
                    .ToList();

                context.Set<AuditEntry>().Add(new AuditEntry
                {
                    TableName = entry.Entity.GetType().Name,
                    RecordId = recordId,
                    Operation = entry.State.ToString(),
                    ChangeDate = now,
                    ChangedById = _currentUserId,
                    OldValues = entry.State == EntityState.Modified ? JsonSerializer.Serialize(entry.OriginalValues.ToObject()) : null,
                    NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject()),
                    ChangedProperties = modifiedProps.Any() ? JsonSerializer.Serialize(modifiedProps) : null
                });
            }
        }
    }

}