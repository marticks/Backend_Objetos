using Microsoft.AspNetCore.Mvc;
using WebApiObjetos.Domain;
using WebApiObjetos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiObjetos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }



        public IActionResult Login(UserDTO user)
        {
            return null;

        }


    }
}
