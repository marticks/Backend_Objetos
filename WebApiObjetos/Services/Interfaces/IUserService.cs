using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiObjetos.Domain;
using WebApiObjetos.Models.Entities;

namespace WebApiObjetos.Services.Interfaces
{
    public interface IUserService
    {

        Task<UserDTO> Login(LoginDTO user);

        Task<bool> SignIn(UserDTO user);

        Task DeleteUser(LoginDTO user);

        Task<UserDTO> RefreshTokens(string token, string refreshToken);
    }
}
