using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    // Enables design-time creation of DatabaseContext for EF Core tools (migrations)
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            // Design-time connection string: match API appsettings.json
            var connectionString = "Server=WIN-8I3BJGN4E51\\SQLEXPRESS;Database=mauiappx;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(connectionString);

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}