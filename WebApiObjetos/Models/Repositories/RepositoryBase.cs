using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Data;
using WebApiObjetos.Models.Repositories.Interfaces;

namespace WebApiObjetos.Models.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext applicationDbContext;

        public RepositoryBase(DbContext dbContext)
        {
            this.applicationDbContext = dbContext;
        }

        public async virtual Task Add(TEntity entity)
        {
            try
            {
                applicationDbContext.Add<TEntity>(entity);
                await applicationDbContext.SaveChangesAsync();//debería usar savechanges async ? rta:tengo que hacer todo asyncrono hasta el controller y ya funca
            }
            catch (Exception e)
            {
            }
        }

        public void Delete(TEntity entity)
        {
            //applicationDbContext.en

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

        public TEntity GetById(long id)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity dbEntity, TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
