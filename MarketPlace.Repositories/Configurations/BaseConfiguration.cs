namespace MarketPlace.Repositories.Configurations
{
    abstract class BaseConfiguration<TEntity> where TEntity : class
    {
        protected BaseConfiguration(DatabaseContext databaseContext) { _DatabaseContext = databaseContext; }

        protected DatabaseContext _DatabaseContext { get; set; }
    }
}
