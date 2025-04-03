using App.Application.Contracts.Persistence;
using App.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Persistence
{
    public class GenericRepository<T, TKey>(AppDbContext context) : IGenericRepository<T, TKey> where T : BaseEntity<TKey> where TKey : struct
    {
        protected AppDbContext Context = context;

        private readonly DbSet<T> _dbSet = context.Set<T>();

        public Task<List<T>> GetAllAsync() => _dbSet.AsNoTracking().ToListAsync();

        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);
        }

        public Task<bool> AnyAsync(TKey id) => _dbSet.AnyAsync(x => x.Id.Equals(id));

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => _dbSet.AnyAsync(predicate);

        public ValueTask<T?> GetByIdAsync(TKey id) => _dbSet.FindAsync(id);

        public async ValueTask AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

    }
}
