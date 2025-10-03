using MarketPlace.Repositories.Configurations;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class DatabaseContext : BaseDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration(this));
            modelBuilder.ApplyConfiguration(new CompanyConfiguration(this));
            modelBuilder.ApplyConfiguration(new AddressConfiguration(this));
            modelBuilder.ApplyConfiguration(new BrandConfiguration(this));
            modelBuilder.ApplyConfiguration(new CategoryConfiguration(this));
            modelBuilder.ApplyConfiguration(new VariationTypeConfiguration(this));
            modelBuilder.ApplyConfiguration(new VariationValueConfiguration(this));
            modelBuilder.ApplyConfiguration(new ProductConfiguration(this));
            modelBuilder.ApplyConfiguration(new AuctionConfiguration(this));
            modelBuilder.ApplyConfiguration(new AuctionProductConfiguration(this));
            modelBuilder.ApplyConfiguration(new BidConfiguration(this));
            modelBuilder.ApplyConfiguration(new OrderConfiguration(this));
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration(this));
        }

        public override int SaveChanges()
        {
            UpdateAuditableProperties();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditableProperties();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateAuditableProperties();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditableProperties();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
