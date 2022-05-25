using System.Linq.Expressions;
using CityDiscoverTourist.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CityDiscoverTourist.Data.IRepositories
{
    public interface IGenericRepository<T, K> where T : class
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> expression);

        DbSet<T> DbSet();

        Task<T> Get(K id);

        Task<T> Add(T entity);

        Task<T> Update(T entity);
        Task<T> UpdateFields(T entity, params Expression<Func<T, object>>[] properties);
        Task<T> NoneUpdateFields(T entity, params Expression<Func<T, object>>[] noneUpdateFields);

        Task<T> Delete(K id);
    }
}