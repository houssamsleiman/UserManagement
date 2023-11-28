using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UserManagement.Manager.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get();
        Task<IEnumerable<T>> GetAsync();
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
