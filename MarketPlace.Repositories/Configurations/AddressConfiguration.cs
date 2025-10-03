using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class AddressConfiguration : BaseConfiguration<Address>, IEntityTypeConfiguration<Address>
    {
        public AddressConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("MARKETPLACE_Address");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FirstLine).HasMaxLength(200);
            builder.Property(a => a.SecondLine).HasMaxLength(200);
            builder.Property(a => a.City).HasMaxLength(100);
            builder.Property(a => a.State).HasMaxLength(100);
            builder.Property(a => a.PostalCode).HasMaxLength(20);
        }
    }
}
