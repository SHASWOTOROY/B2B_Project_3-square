namespace Threesquare.Core.Repositories
{
    public interface IRepositorySession
    {
        Task<int> Commit();
    }
}
