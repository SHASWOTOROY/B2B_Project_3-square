using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Models;

namespace Threesquare.Core.Repositories.EntityFramework
{
    public abstract class BaseDatabaseContext : DbContext
    {
        public BaseDatabaseContext(DbContextOptions options) : base(options) { }

        protected void UpdateAuditableProperties()
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.ModifiedOn = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Entity.ModifiedOn = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
