using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApiObjetos.Models.Repositories.Interfaces
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        Task<TEntity> GetById(int id);
        Task Add(TEntity entity);
        void Update(TEntity dbEntity, TEntity entity);
        Task Delete(TEntity entity);
        IList<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
    }
}
