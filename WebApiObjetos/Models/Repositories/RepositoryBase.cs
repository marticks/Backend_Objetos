using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Data;
using WebApiObjetos.Models.Entities;
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

        public async Task Delete(TEntity entity)
        {
            try
            {
                applicationDbContext.Remove<TEntity>(entity);
                await applicationDbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
          /* var Users = applicationDbContext.Users.ToList();
            applicationDbContext.Model.
           return Users;
            //dbContext
            */
            throw new NotImplementedException();
        }

        public void Update(TEntity dbEntity, TEntity entity)
        {
            throw new NotImplementedException();
        }

        TEntity IRepository<TEntity>.GetById(int id)
        {
            var result = applicationDbContext.Find<TEntity>(id);
            return result ;
        }
    }
}
