using Microsoft.AspNetCore.Mvc;
using WebApiObjetos.Domain;
using WebApiObjetos.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiObjetos.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using WebApiObjetos.Properties;
using System.Resources;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

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

        [HttpPost]
        [Route("login"), AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model State"); // supuestamente las excepciones son lentas, es mejor retornar el BadRequest.

            var obtainedUser = await userService.Login(user);

            if (obtainedUser == null)
                return Unauthorized();

            //await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, principal); agregado para mirar mañana
            //await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

            return Ok(obtainedUser);
        }


        [Route("SignIn")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UserDTO user)
        {
            if (!ModelState.IsValid)
                throw new InvalidOperationException("Invaid Model");

           var success = await userService.SignIn(user);
            if (!success)
                return Conflict("El usuario ya existe");

            return Created("El usuario ha sido registrado exitosamente",user);
        }


        [Route("Delete")]
        //[ValidateAntiForgeryToken] // se ve que todas las forms tienen un token autogenerado, que necesitas validar de este lado con esta linea, asi evitas que otra página suba una form aca. 
        //[RequireHttps]//obliga a conectarse por https y sino te fuerza a hacerlo. habilitar firma ssl en properties
        [HttpDelete,Authorize]
        public async Task<IActionResult> DeleteUser([FromBody] LoginDTO user)  /// Me parece bien que para borrar el usuario ademas del token requiera usuario y contraseña
        {
            var context = HttpContext.User; // con esto accedes a las claims
            if (!ModelState.IsValid)
                throw new InvalidOperationException("Invaid Model");

            await userService.DeleteUser(user);

            return Ok("Su usuario ha sido eliminado exitosamente");
        }


        [HttpPost]
        [Route("Refresh")]
        //Se almacena el refresh token junto con el usuario, esto obliga a que solo haya una sesión iniciada al a vez, ya que hay un solo refresh token
        public async Task<IActionResult> RefreshToken(string token, string refreshToken) // le envio el token en el body para que pueda extraer las claims de ahi.
        {
            if (!ModelState.IsValid)
                throw new InvalidOperationException("Invaid Model");

            var newTokens = await userService.RefreshTokens(token, refreshToken);

            return Ok(newTokens);

        }


    }
}
