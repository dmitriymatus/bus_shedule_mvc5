using System;
using System.Collections.Generic;
using System.Data.Entity;
using Domain.Abstract;
using System.Linq.Expressions;
using System.Linq;

namespace Domain.Concrete
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity:class
    {
        protected readonly SheduleDbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public GenericRepository(SheduleDbContext _context)
        {
            context = _context;
            dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.ToList();
        }

        public TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }

        public void Delete(object id)
        {
            TEntity entity = dbSet.Find(id);
            if(entity != null)
            {
                dbSet.Remove(entity);
                context.SaveChanges();
            }
        }
    }
}
