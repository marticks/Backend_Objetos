using Microsoft.AspNetCore.Mvc;
using WebApiObjetos.Domain;
using WebApiObjetos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Models.Entities;

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


        [Route("login")]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
                throw new InvalidOperationException("Invalid Model");//esta excepcion es usada cuando se trata de realizar una operación con un objeto 
                                                                     //con un estado invalido, creo que va bien como excepcion para este caso 
            await userService.Login(user);
            return Ok("logeo papa, retornar token");
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] User user)
        {
            if (!ModelState.IsValid)
                throw new InvalidOperationException("Invaid Model");

            await userService.SignIn(user);
            return Ok("El usuario ha sido registrado exitosamente");
        }

        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] User user)
            {
            if (!ModelState.IsValid)
                throw new InvalidOperationException("Invaid Model");

            await userService.DeleteUser(user);

            return Ok("Su usuario ha sido eliminado exitosamente");
        }


    }
}
