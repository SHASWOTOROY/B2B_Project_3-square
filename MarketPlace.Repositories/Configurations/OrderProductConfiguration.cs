using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class OrderProductConfiguration : BaseConfiguration<OrderProduct>, IEntityTypeConfiguration<OrderProduct>
    {
        public OrderProductConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.ToTable("MARKETPLACE_OrderProduct");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ProductName).HasMaxLength(100);
            builder.OwnsMany<ProductVariation>(m => m.Variations, pvBuilder =>
            {
                pvBuilder.ToTable("MARKETPLACE_OrderProductVariation");
                pvBuilder.WithOwner().HasForeignKey("OrderProductId");
                pvBuilder.Property(v => v.TypeId).IsRequired();
                pvBuilder.Property(v => v.ValueId).IsRequired();
                pvBuilder.Property(v => v.Type).HasMaxLength(100).IsRequired();
                pvBuilder.Property(v => v.Value).HasMaxLength(100).IsRequired();
                pvBuilder.HasKey("OrderProductId", "TypeId", "ValueId");
            });

            builder.HasOne(m => m.Product).WithMany().HasForeignKey(m => m.ProductId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => new { m.OrderId });
            builder.HasIndex(m => new { m.ProductId });
        }
    }
}
