using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class CategoryConfiguration : BaseConfiguration<Category>, IEntityTypeConfiguration<Category>
    {
        public CategoryConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("MARKETPLACE_Category");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name).HasMaxLength(100);

            builder.HasOne(m => m.ParentCategory).WithMany().HasForeignKey(m => m.ParentCategoryId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => m.Name).IsUnique();
            builder.HasIndex(m => new { m.ParentCategoryId });
            builder.HasIndex(m => new { m.IsActive });
        }
    }
}
