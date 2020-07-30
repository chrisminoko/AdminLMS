using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Repository
{
   public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();
        TEntity Get(object id);
        bool Insert(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);
        void Delete(List<TEntity> entities);
        IQueryable<TEntity> Find(Func<TEntity, bool> predicate);
    }
}
