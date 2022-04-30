using Microsoft.EntityFrameworkCore;
using Repository.interfaces;

namespace Repository.repositories
{
    public class DataBaseRepository<TEntity, TContext> : IBaseRepository<TEntity>
        where TEntity:  class
        where TContext : DbContext
    {
        protected readonly TContext _repositoryContext;
        public DataBaseRepository(TContext context)
        {
            _repositoryContext = context;
        }

        public virtual async Task AddAsync(TEntity item)
        {
            await _repositoryContext.Set<TEntity>().AddAsync(item);
            await SaveAsync();
        }

        public virtual async Task AddRangeAsync(ICollection<TEntity> items)
        {
            await _repositoryContext.Set<TEntity>().AddRangeAsync(items);
            await SaveAsync();
        }

        public virtual IQueryable<TEntity> GetEntityQuery()
        {
            IEnumerable<TEntity> result = _repositoryContext.Set<TEntity>();

            return (IQueryable<TEntity>)result;
        }

        public virtual async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> RemoveRangeAsync(Func<TEntity, bool> predicate)
        {
            var entities = _repositoryContext.Set<TEntity>().Where(predicate);

            var removeRangeAsync = entities as TEntity[] ?? entities.ToArray();
            _repositoryContext.Set<TEntity>().RemoveRange(removeRangeAsync);
            await SaveAsync();
            return removeRangeAsync;
        }

        public virtual async Task UpdateAsync(TEntity item)
        {

            _repositoryContext.Entry(item).State = EntityState.Modified;
            await SaveAsync();
        }
    }
}
