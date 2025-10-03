using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class BidConfiguration : BaseConfiguration<Bid>, IEntityTypeConfiguration<Bid>
    {
        public BidConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.ToTable("MARKETPLACE_Bid");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Status).HasColumnType("varchar(20)").HasConversion<string>().IsRequired();

            builder.HasOne(m => m.Auction).WithMany().HasForeignKey(m => m.AuctionId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m => m.AuctionProduct).WithMany().HasForeignKey(m => m.AuctionProductId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m => m.SellerCompany).WithMany().HasForeignKey(m => m.SellerCompanyId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => new { m.AuctionId });
            builder.HasIndex(m => new { m.AuctionProductId });
            builder.HasIndex(m => new { m.SellerCompanyId });
            builder.HasIndex(m => new { m.SellerUserId });
            builder.HasIndex(m => new { m.Status });
        }
    }
}
