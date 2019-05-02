using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                throw e;
            }
        }

        public async Task Delete(TEntity entity)
        {
            try
            {
                applicationDbContext.Remove<TEntity>(entity);
                await applicationDbContext.SaveChangesAsync();
            }
            catch (Exception e)
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
            applicationDbContext.Update(entity);
            applicationDbContext.SaveChanges();

            throw new NotImplementedException();
        }

        public async Task<TEntity> GetById(int id)
        {
            var result = await applicationDbContext.FindAsync<TEntity>(id);
            return result;
        }

        public IList<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate) /// método base para busqueda dada una expresión
        {
                IList<TEntity> query = applicationDbContext.Set<TEntity>().Where(predicate).ToList();
                return query;
        }

    }
}
