using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Models.Entities;

namespace WebApiObjetos.Services.Interfaces
{
    public interface IUserService
    {

        Task<User> Login(User user);

        Task<bool> SignIn(User user);

        Task DeleteUser(User user);
    }
}
