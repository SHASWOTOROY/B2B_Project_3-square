using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class VariationValueConfiguration : BaseConfiguration<VariationValue>, IEntityTypeConfiguration<VariationValue>
    {
        public VariationValueConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<VariationValue> builder)
        {
            builder.ToTable("MARKETPLACE_VariationValue");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name).HasMaxLength(100);

            builder.HasOne(m => m.VariationType).WithMany().HasForeignKey(m => m.VariationTypeId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => new { m.VariationTypeId, m.Name }).IsUnique();
            builder.HasIndex(m => new { m.IsActive });
        }
    }
}
