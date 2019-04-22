﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Data;
using WebApiObjetos.Models.Entities;
using WebApiObjetos.Models.Repositories.Interfaces;

namespace WebApiObjetos.Models.Repositories
{
    // Hereda de repositoryBase e implementa UserRepository para consultas específicas.
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext) { }


        //aca debería tener las consultas específicas(devolveme los 10 primeros ordenados por X y eso.)
        //no creo que se pueda hacer con una lambda como el where.

        public async Task<User> GetUser(User user)
        {
            try
            {
                var result = await applicationDbContext.Set<User>().
                    Where(x => x.UserName.Equals(user.UserName)).FirstOrDefaultAsync<User>();

                applicationDbContext.SaveChanges();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
            //myContext.HugeEntity.Select(entity => new { entity.FirstName, entity.Address1 }); 
            //con esto podes realizar busquedas sin traerte todas las columnas de la tabla (anonymous types)
        }

    }
}
