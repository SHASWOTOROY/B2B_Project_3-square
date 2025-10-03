using Threesquare.Core.Models;

namespace Threesquare.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task Create(TEntity entity, CancellationToken cancellationToken = default);
        Task Create(List<TEntity> entities, CancellationToken cancellationToken = default);

        Task<TEntity?> Read(object key, CancellationToken cancellationToken = default);
        Task<TEntity?> Read(object key, List<string> includeProperties, CancellationToken cancellationToken = default);

        Task<List<TEntity>> ReadMany(CancellationToken cancellationToken = default);
        Task<PagedEntities<TEntity>> ReadMany(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        void Update(TEntity entity);
        void Update(List<TEntity> entities);

        Task Delete(object key, CancellationToken cancellationToken = default);
        void Delete(TEntity entity);
        void Delete(List<TEntity> entities);

        Task<int> Count(CancellationToken cancellationToken = default);
    }
}
