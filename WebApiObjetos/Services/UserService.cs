using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Models.Entities;
using WebApiObjetos.Models.Repositories;
using WebApiObjetos.Models.Repositories.Interfaces;
using WebApiObjetos.Services.Interfaces;

namespace WebApiObjetos.Services
{
    //representa las columnas de la tabla, tiene sus parametros
    public class UserService : IUserService
    {
        private IUserRepository userRepo;

        public UserService(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
        }


        public async Task DeleteUser(User user)
        {
            /*
            int id = 1;
            user = userRepo.GetById(id); esto era una prueba y funciona bien el getbyId
            */

            user = await userRepo.GetUser(user);

            await userRepo.Delete(user);
        }

        public async Task Login(User user)
        {
            var result = await userRepo.GetUser(user);
            if (result != null)
                return;
            //generar token y devolverlo
        }

        public async Task SignIn(User user)
        {
            await userRepo.Add(user);
        }




    }
}
