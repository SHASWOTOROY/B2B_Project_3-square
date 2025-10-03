namespace Threesquare.Core.Repositories.EntityFramework
{
    public abstract class BaseRepositorySession : IRepositorySession, IDisposable
    {
        public BaseRepositorySession(BaseDatabaseContext context)
        {
            _Context = context;
        }

        public async Task<int> Commit()
        {
            return await _Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Do not attempt to save during Dispose to avoid using a disposed service provider/context
            // Rely on explicit Commit() calls by application code when needed
            _Context.Dispose();
            GC.SuppressFinalize(this);
        }

        internal BaseDatabaseContext _Context;
    }
}
