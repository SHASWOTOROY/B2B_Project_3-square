using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class AuctionProductConfiguration : BaseConfiguration<AuctionProduct>, IEntityTypeConfiguration<AuctionProduct>
    {
        public AuctionProductConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<AuctionProduct> builder)
        {
            builder.ToTable("MARKETPLACE_AuctionProduct");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ProductName).HasMaxLength(100);
            builder.Ignore(m => m.IsBidAccepted);
            builder.OwnsMany<ProductVariation>(m => m.Variations, pvBuilder =>
            {
                pvBuilder.ToTable("MARKETPLACE_AuctionProductVariation");
                pvBuilder.WithOwner().HasForeignKey("AuctionProductId");
                pvBuilder.Property(v => v.TypeId).IsRequired();
                pvBuilder.Property(v => v.ValueId).IsRequired();
                pvBuilder.Property(v => v.Type).HasMaxLength(100).IsRequired();
                pvBuilder.Property(v => v.Value).HasMaxLength(100).IsRequired();
                pvBuilder.HasKey("AuctionProductId", "TypeId", "ValueId");
            });

            builder.HasOne(m => m.Product).WithMany().HasForeignKey(m => m.ProductId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m => m.AcceptedBid).WithMany().HasForeignKey(m => m.AcceptedBidId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => new { m.AuctionId });
            builder.HasIndex(m => new { m.ProductId });
        }
    }
}
