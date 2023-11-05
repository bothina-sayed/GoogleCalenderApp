using GoogleCalenderApplication.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalenderApplication.Domain.Abstractions
{
    public interface IGenericRepository<T> where T : class
    {
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        IReadOnlyList<T> Get();
        (IQueryable<T> data, int count) GetWithSpec(BaseSpecifications<T> specifications);
        Task<T?> GetById(params object[] idValues);
        (T? data, int count) GetEntityWithSpec(BaseSpecifications<T> specifications);
        Task<T?> GetObj(Expression<Func<T, bool>> filter);
        Task<bool> IsExist(Expression<Func<T, bool>> filter);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        Task<bool> Save();
    }
}
