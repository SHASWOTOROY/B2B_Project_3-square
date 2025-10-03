using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class OrderConfiguration : BaseConfiguration<Order>, IEntityTypeConfiguration<Order>
    {
        public OrderConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("MARKETPLACE_Order");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.OrderId).HasMaxLength(50);
            builder.HasOne(o => o.DeliveryAddress).WithMany().HasForeignKey(o => o.DeliveryAddressId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(m => m.Status).HasColumnType("varchar(20)").HasConversion<string>().IsRequired();

            builder.HasOne(m => m.BuyerCompany).WithMany().HasForeignKey(m => m.BuyerCompanyId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m => m.SellerCompany).WithMany().HasForeignKey(m => m.SellerCompanyId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(m => m.OrderProducts).WithOne().HasForeignKey(m => m.OrderId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => new { m.OrderDate });
            builder.HasIndex(m => new { m.OrderId });
            builder.HasIndex(m => new { m.BuyerCompanyId });
            builder.HasIndex(m => new { m.SellerCompanyId });
            builder.HasIndex(m => new { m.BuyerUserId });
            builder.HasIndex(m => new { m.SellerUserId });
            builder.HasIndex(m => new { m.Status });
        }
    }
}
