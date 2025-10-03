using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class VariationTypeConfiguration : BaseConfiguration<VariationType>, IEntityTypeConfiguration<VariationType>
    {
        public VariationTypeConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<VariationType> builder)
        {
            builder.ToTable("MARKETPLACE_VariationType");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name).HasMaxLength(100);

            builder.HasMany(m => m.Values).WithOne().HasForeignKey(m => m.VariationTypeId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => m.Name).IsUnique();
            builder.HasIndex(m => new { m.IsActive });
        }
    }
}
