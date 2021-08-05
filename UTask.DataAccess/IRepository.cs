using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UTask.DataAccess
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);

        void Delete(IEnumerable<T> entities);

        void Delete(T entity);

        List<T> GetAll();

        T GetById(Guid id);

        IQueryable<T> Query();

        IQueryable<T> Query(Expression<Func<T, bool>> expression);

        T Update(T entity);
    }
}