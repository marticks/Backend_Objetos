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

        [HttpPost] // Recordar usar estas notations+
        [Route("login"), AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model State"); // supuestamente las excepciones son lentas, es mejor retornar el BadRequest.

                //throw new InvalidOperationException("Invalid Model");esta excepcion es usada cuando se trata de realizar una operación con un objeto 
                                                                     //con un estado invalido, creo que va bien como excepcion para este caso 
            var obtainedUser = await userService.Login(user);


            if (obtainedUser == null)
                return Unauthorized();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Resources.Encription_Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claim = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            // podes agregar otras sub y claims, es un arreglo, pero es mejor crear una claim nueva y chau.
            new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub,"sass"),
            new Claim("edad", "user.edad")
    };

            var token = new JwtSecurityToken(
                issuer: Resources.Issuer,
                audience: Resources.Audience,
                claims: claim,
                expires: DateTime.UtcNow.AddHours(Int32.Parse(Resources.Token_Duration)),
                signingCredentials: cred
                );

            UserDTO userdto = new UserDTO()
            {
                UserName = user.UserName,
                Password = user.Password,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
        };


            return Ok(userdto);
        }


        [Route("SignIn")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] User user)
        {
            if (!ModelState.IsValid)
                throw new InvalidOperationException("Invaid Model");

           var success = await userService.SignIn(user);
            if (!success)
                return Conflict("El usuario ya existe");

            return Created("El usuario ha sido registrado exitosamente",user);
        }


        [Route("Delete")]
        [HttpDelete,Authorize]
        public async Task<IActionResult> DeleteUser([FromBody] User user)
        {
            var context = HttpContext.User; // con esto accedes a las claims
            if (!ModelState.IsValid)
                throw new InvalidOperationException("Invaid Model");

            await userService.DeleteUser(user);

            return Ok("Su usuario ha sido eliminado exitosamente");
        }





    }
}
