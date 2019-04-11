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


        public void DeleteUser()
        {
            throw new NotImplementedException();
        }

        public void Login()
        {
            //userRepo.Add()
            throw new NotImplementedException();
        }

        public async Task SignIn(User user)
        {
           await userRepo.Add(user);
        }
    }
}
