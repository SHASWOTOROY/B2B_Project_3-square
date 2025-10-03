using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class ProductConfiguration : BaseConfiguration<Product>, IEntityTypeConfiguration<Product>
    {
        public ProductConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("MARKETPLACE_Product");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name).HasMaxLength(100);
            builder.Property(m => m.ShortDescription).HasMaxLength(500);
            builder.Property(m => m.Description).HasColumnType("TEXT");
            builder.Property(m => m.Photo).HasMaxLength(500);

            // Store per-product variations in dedicated tables
            builder.OwnsMany<ProductVariationType>(m => m.Variations, vtBuilder =>
            {
                vtBuilder.ToTable("MARKETPLACE_ProductVariationType");
                vtBuilder.WithOwner().HasForeignKey("ProductId");
                vtBuilder.Property(vt => vt.TypeId).IsRequired().ValueGeneratedNever();
                vtBuilder.Property(vt => vt.Name).HasMaxLength(100).IsRequired();
                // Composite key ensures uniqueness per product/type
                vtBuilder.HasKey("ProductId", "TypeId");

                vtBuilder.OwnsMany<ProductVariationValue>(mpvt => mpvt.Values, vvBuilder =>
                {
                    vvBuilder.ToTable("MARKETPLACE_ProductVariationValue");
                    // include owner FKs in child table
                    vvBuilder.WithOwner().HasForeignKey("ProductId", "TypeId");
                    vvBuilder.Property(vv => vv.ValueId).IsRequired().ValueGeneratedNever();
                    vvBuilder.Property(vv => vv.Name).HasMaxLength(100).IsRequired();
                    // Composite key includes type key and value id
                    vvBuilder.HasKey("ProductId", "TypeId", "ValueId");
                });
            });

            builder.HasOne(m => m.Category).WithMany().HasForeignKey(m => m.CategoryId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m => m.Brand).WithMany().HasForeignKey(m => m.BrandId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(m => m.Companies).WithMany(c => c.Products).UsingEntity("MARKETPLACE_ProductCompany",
                r => r.HasOne(typeof(Company)).WithMany().HasForeignKey("CompanyId").HasPrincipalKey(nameof(Company.Id)),
                l => l.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId").HasPrincipalKey(nameof(Product.Id)),
                j => j.HasKey("CompanyId", "ProductId"));

            builder.HasIndex(m => new { m.CategoryId });
            builder.HasIndex(m => new { m.BrandId });
            builder.HasIndex(m => new { m.Name });
            builder.HasIndex(m => new { m.IsActive });
        }
    }
}
