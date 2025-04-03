using App.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace App.Persistence.Interceptors
{
    public class AuditDbContextInterceptor<TKey> : SaveChangesInterceptor
    {

        private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> Behaviors = new()
        {
            {EntityState.Added, AddBehavior},
            {EntityState.Modified, ModifiedBehavior},
            {EntityState.Deleted, DeletedBehavior}
        };

        private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.CreatedAt = DateTime.UtcNow;
            auditEntity.IsActive = true;
            context.Entry(auditEntity).Property(x => x.UpdatedAt).IsModified = false;
            context.Entry(auditEntity).Property(x => x.UpdatedBy).IsModified = false;
            context.Entry(auditEntity).Property(x => x.DeletedAt).IsModified = false;
            context.Entry(auditEntity).Property(x => x.DeletedBy).IsModified = false;

        }

        private static void ModifiedBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.UpdatedAt = DateTime.UtcNow;
            context.Entry(auditEntity).Property(x => x.CreatedAt).IsModified = false;
            context.Entry(auditEntity).Property(x => x.CreatedBy).IsModified = false;
            context.Entry(auditEntity).Property(x => x.DeletedAt).IsModified = false;
            context.Entry(auditEntity).Property(x => x.DeletedBy).IsModified = false;
        }

        private static void DeletedBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.DeletedAt = DateTime.UtcNow;
            auditEntity.IsDeleted = true;
            auditEntity.IsActive = false;
            context.Entry(auditEntity).Property(x => x.CreatedAt).IsModified = false;
            context.Entry(auditEntity).Property(x => x.CreatedBy).IsModified = false;
            context.Entry(auditEntity).Property(x => x.UpdatedAt).IsModified = false;
            context.Entry(auditEntity).Property(x => x.UpdatedBy).IsModified = false;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
            {
                if (entityEntry.Entity is not IAuditEntity auditEntity) continue;

                if (entityEntry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted)) continue;

                Behaviors[entityEntry.State](eventData.Context, auditEntity);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
