using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Models.Repositories.Interfaces;

namespace WebApiObjetos.Models.Repositories
{
    public class RepositoryBase<TEntity> : IRepository <TEntity> where TEntity : class
    {
        private DbContext dbContext;

        public RepositoryBase(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(TEntity entity)
        {
            //using (var context = new ApplicationDBContext(DbContextOptions < DbContex > options)) //// esta inyección de dependencias ya la hace ? ///ya tengo el DB CONTEXT, NO USAR USING CARA DE VERGA
            //{ }
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetAll()
        {

            //dbContext

            throw new NotImplementedException();
        }

        public void Update(TEntity dbEntity, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
