using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class CompanyConfiguration : BaseConfiguration<Company>, IEntityTypeConfiguration<Company>
    {
        public CompanyConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("MARKETPLACE_Company");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name).HasMaxLength(100);
            builder.Property(m => m.Email).HasMaxLength(100);
            builder.Property(m => m.Phone).HasMaxLength(20);
            builder.Property(m => m.Website).HasMaxLength(100);

            builder.HasOne(m => m.Address)
                   .WithMany()
                   .HasForeignKey(m => m.AddressId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(m => m.Products).WithMany(p => p.Companies).UsingEntity("MARKETPLACE_ProductCompany",
                r => r.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId").HasPrincipalKey(nameof(Product.Id)),
                l => l.HasOne(typeof(Company)).WithMany().HasForeignKey("CompanyId").HasPrincipalKey(nameof(Company.Id)));

            builder.HasIndex(m => new { m.Phone });
            builder.HasIndex(m => new { m.Email });
            builder.HasIndex(m => new { m.IsActive });
            builder.HasIndex(m => new { m.AddressId });
        }
    }
}
