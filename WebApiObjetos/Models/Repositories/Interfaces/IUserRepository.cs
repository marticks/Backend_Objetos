﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Models.Entities;

namespace WebApiObjetos.Models.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUser(User user);

        Task<User> GetUserByUserName(string user);

        Task<string> GetRefreshTokenAsync(string username);

        Task SaveRefreshToken(string userName, string refreshToken);
    }
}
