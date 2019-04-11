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
    // Hereda de repositoryBase e implementa UserRepository para consultas específicas.
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext){}

        //aca debería tener las consultas específicas(devolveme los 10 primeros ordenados por X y eso.)
        //no creo que se pueda hacer con una lambda como el where.
        //un get con ciertas características tampoco se puede hacer, dame todas las empresas con organization = 10 y eso.
    }
}
