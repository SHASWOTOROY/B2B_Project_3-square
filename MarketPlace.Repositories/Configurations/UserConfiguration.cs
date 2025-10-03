using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlace.Repositories.Configurations
{
    internal class UserConfiguration : BaseConfiguration<User>, IEntityTypeConfiguration<User>
    {
        public UserConfiguration(DatabaseContext databaseContext) : base(databaseContext) { }

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("MARKETPLACE_User");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name).HasMaxLength(100);
            builder.Property(m => m.Email).HasMaxLength(100);
            builder.Property(m => m.Phone).HasMaxLength(20);
            builder.Property(m => m.Username).HasMaxLength(100);
            builder.Property(m => m.Password).HasMaxLength(100);

            builder.HasOne(m => m.Company).WithMany().HasForeignKey(m => m.CompanyId).IsRequired().OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => new { m.CompanyId });
            builder.HasIndex(m => new { m.Phone });
            builder.HasIndex(m => new { m.Email });
            builder.HasIndex(m => new { m.Username });
            builder.HasIndex(m => new { m.IsActive });
        }
    }
}
