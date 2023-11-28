using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Manager.Interfaces;

namespace UserManagement.Manager.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Members 
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor 
        public Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods 
        public void Add(T entity)
        {
            _unitOfWork._context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _unitOfWork._context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> Get()
        {
            return _unitOfWork._context.Set<T>().AsEnumerable<T>();
        }
        public Task<IEnumerable<T>> GetAsync()
        {
            return Task.FromResult(_unitOfWork._context.Set<T>().AsEnumerable<T>());
        }

        public IEnumerable<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _unitOfWork._context.Set<T>().Where(predicate).AsEnumerable<T>();
        }

        public Task<IEnumerable<T>> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return Task.FromResult(_unitOfWork._context.Set<T>().Where(predicate).AsEnumerable<T>());
        }

        public void Update(T entity)
        {
            _unitOfWork._context.Entry(entity).State = EntityState.Modified;
            _unitOfWork._context.Set<T>().Update(entity);
        }
        #endregion
    }
}
