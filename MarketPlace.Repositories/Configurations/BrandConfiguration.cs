using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class BrandConfiguration : BaseConfiguration<Brand>, IEntityTypeConfiguration<Brand>
    {
        public BrandConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("MARKETPLACE_Brand");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name).HasMaxLength(100);

            builder.HasIndex(m => m.Name).IsUnique();
            builder.HasIndex(m => new { m.IsActive });
        }
    }
}
