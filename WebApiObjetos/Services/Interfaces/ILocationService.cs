using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiObjetos.Services.Interfaces
{
    public interface ILocationService
    {
        void GetLocations();

        void GetLocation();

        void DeleteLocation();

        void ModifyLocation();

    }
}
