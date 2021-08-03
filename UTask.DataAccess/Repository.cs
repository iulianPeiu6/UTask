using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UTask.DataAccess.Context;

namespace UTask.DataAccess
{
    public class Repository<T> : IRepository<T> 
        where T : class
    {
        private readonly UTaskContext uTaskContext;

        private readonly DbSet<T> dbSet;

        public Repository(UTaskContext uTaskContext)
        {
            this.uTaskContext = uTaskContext;
            dbSet = uTaskContext.Set<T>();
        }

        public T Add(T entity)
        {
            return dbSet.Add(entity).Entity;
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression).AsQueryable();
        }

        public IQueryable<T> Query()
        {
            return dbSet.AsQueryable();
        }

        public T Update(T entity)
        {
            var updatedEntity = dbSet.Attach(entity).Entity;
            uTaskContext.Entry(entity).State = EntityState.Modified;
            return updatedEntity;
        }
    }
}
