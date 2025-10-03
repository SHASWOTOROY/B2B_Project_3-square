using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class AuctionConfiguration : BaseConfiguration<Auction>, IEntityTypeConfiguration<Auction>
    {
        public AuctionConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.ToTable("MARKETPLACE_Auction");

            builder.HasKey(m => m.Id);

            builder.HasOne(a => a.DeliveryAddress).WithMany().HasForeignKey(a => a.DeliveryAddressId).OnDelete(DeleteBehavior.NoAction);
            builder.Property(m => m.Status).HasColumnType("varchar(20)").HasConversion<string>().IsRequired();

            builder.HasOne(m => m.BuyerCompany).WithMany().HasForeignKey(m => m.BuyerCompanyId).IsRequired().OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(m => m.AuctionProducts).WithOne().HasForeignKey(m => m.AuctionId);

            builder.HasIndex(m => new { m.BuyerCompanyId });
            builder.HasIndex(m => new { m.BuyerUserId });
            builder.HasIndex(m => new { m.EndTime });
            builder.HasIndex(m => new { m.Status });
        }
    }
}
