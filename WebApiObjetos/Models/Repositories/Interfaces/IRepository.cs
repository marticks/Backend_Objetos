using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiObjetos.Models.Repositories.Interfaces
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        Task Add(TEntity entity);
        void Update(TEntity dbEntity, TEntity entity);
        Task Delete(TEntity entity);
    }
}
