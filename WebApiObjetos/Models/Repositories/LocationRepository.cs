using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Models.Entities;
using WebApiObjetos.Models.Repositories.Interfaces;

namespace WebApiObjetos.Models.Repositories
{
    public class LocationRepository : RepositoryBase<Location> , ILocationRepository
    {

        public LocationRepository(DbContext dbContext) : base(dbContext){}

    }
}
