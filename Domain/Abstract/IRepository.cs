using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Abstract
{
    public interface IRepository<TEntity> where TEntity : class 
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter);
        TEntity GetByID(object id);
        void Insert(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
