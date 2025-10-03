using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Threesquare.Core.Models;

namespace Threesquare.Core.Repositories.EntityFramework
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected BaseRepository(DbContext context)
        {
            _Context = context;
            _Collection = _Context.Set<TEntity>();
        }

        public async Task Create(TEntity entity, CancellationToken cancellationToken = default)
            => await _Collection.AddAsync(entity, cancellationToken);
        public async Task Create(List<TEntity> entities, CancellationToken cancellationToken = default)
            => await _Collection.AddRangeAsync(entities, cancellationToken);

        public async Task<TEntity?> Read(object key, CancellationToken cancellationToken = default)
            => await _Collection.FindAsync(key, cancellationToken);
        public async Task<TEntity?> Read(object key, List<string> includeProperties, CancellationToken cancellationToken = default)
            => await GetQueryableWithIncludes(includeProperties).FirstOrDefaultAsync(e => EF.Property<object>(e, GetPrimaryKeyName()).Equals(key), cancellationToken);
        protected async Task<TEntity?> Read(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _Collection.FirstOrDefaultAsync(predicate, cancellationToken);

        public async Task<List<TEntity>> ReadMany(CancellationToken cancellationToken = default) => await _Collection.ToListAsync(cancellationToken);
        protected async Task<List<TEntity>> ReadMany(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _Collection.Where(predicate).ToListAsync(cancellationToken);
        protected async Task<List<TEntity>> ReadMany(Expression<Func<TEntity, bool>> predicate, List<string> includeProperties, CancellationToken cancellationToken = default)
            => await GetQueryableWithIncludes(includeProperties).Where(predicate).ToListAsync(cancellationToken);

        public async Task<PagedEntities<TEntity>> ReadMany(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
            => new PagedEntities<TEntity>
            {
                Items = await _Collection.Skip(Skip(pageNumber, pageSize)).Take(pageSize).ToListAsync(cancellationToken),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = await _Collection.CountAsync(cancellationToken)
            };
        protected async Task<PagedEntities<TEntity>> ReadMany(Expression<Func<TEntity, bool>> predicate, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
            => new PagedEntities<TEntity>()
            {
                Items = await _Collection.Where(predicate).Skip(Skip(pageNumber, pageSize)).Take(pageSize).ToListAsync(cancellationToken),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = await _Collection.CountAsync(predicate, cancellationToken)
            };
        protected async Task<PagedEntities<TEntity>> ReadMany(Expression<Func<TEntity, bool>> predicate, List<string> includeProperties, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = (includeProperties != null && includeProperties.Count >= 0) ? GetQueryableWithIncludes(includeProperties) : _Collection.AsQueryable();
            return new PagedEntities<TEntity>()
            {
                Items = await query.Where(predicate).Skip(Skip(pageNumber, pageSize)).Take(pageSize).ToListAsync(cancellationToken),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = await query.CountAsync(predicate, cancellationToken)
            };
        }

        public void Update(TEntity entity) => _Collection.Update(entity);
        public void Update(List<TEntity> entities) => _Collection.UpdateRange(entities);

        public async Task Delete(object key, CancellationToken cancellationToken = default) { var e = await _Collection.FindAsync(key, cancellationToken); if (e != null) _Collection.Remove(e); }
        public void Delete(TEntity entity) => _Collection.Remove(entity);
        public void Delete(List<TEntity> entities) => _Collection.RemoveRange(entities);
        protected async Task Delete(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => Delete(await ReadMany(predicate, cancellationToken));

        public async Task<int> Count(CancellationToken cancellationToken = default)
            => await _Collection.CountAsync(cancellationToken);
        protected async Task<int> Count(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _Collection.CountAsync(predicate, cancellationToken);

        protected async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
            => await _Collection.CountAsync(predicate, cancellationToken) > 0;

        private string GetPrimaryKeyName()
        {
            return _Collection.EntityType.FindPrimaryKey()?.Properties.First()?.Name ?? "Id";
        }

        private IQueryable<TEntity> GetQueryableWithIncludes(List<string> includeProperties)
        {
            IQueryable<TEntity> query = _Collection.AsQueryable<TEntity>();
            if (includeProperties != null && includeProperties.Count > 0)
                includeProperties.ForEach(includeProperty => query = query.Include(includeProperty));
            return query;
        }

        private int Skip(int pageNumber, int pageSize)
            => (pageNumber - 1) * pageSize;

        private readonly DbContext _Context;
        private readonly DbSet<TEntity> _Collection;
    }
}
