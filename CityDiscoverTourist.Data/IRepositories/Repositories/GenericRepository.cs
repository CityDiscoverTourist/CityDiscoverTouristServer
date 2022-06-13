using System.Linq.Expressions;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Data.IRepositories.Repositories
{
    public class GenericRepository<T, TK> : IGenericRepository<T, TK> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>();
        }

        public DbSet<T> DbSet()
        {
            return _context.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateFields(T entity, params Expression<Func<T, object>>[] properties)
        {
            var dbEntry = _context.Entry(entity);
            foreach (var property in properties)
            {
                dbEntry.Property(property).IsModified = true;
            }
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> NoneUpdateFields(T entity, params Expression<Func<T, object>>[] noneUpdateFields)
        {
            var dbEntry = _context.Update(entity);
            foreach (var property in noneUpdateFields)
            {
                dbEntry.Property(property).IsModified = false;
            }
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(TK id)
        {
            var entity = await _context.Set<T>().FindAsync(id).ConfigureAwait(false);
            if (entity == null) return null;

            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> Get(TK id)
        {
            return (await _context.Set<T>().FindAsync(id))!;
        }

        public async Task<T> Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsNoTracking();
        }
    }
}