using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApiObjetos.Domain;
using WebApiObjetos.Models.Entities;
using WebApiObjetos.Models.Repositories;
using WebApiObjetos.Models.Repositories.Interfaces;
using WebApiObjetos.Properties;
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


        public async Task<bool> DeleteUser(LoginDTO user)
        {
            /*
            int id = 1;
            user = userRepo.GetById(id); esto era una prueba y funciona bien el getbyId
            */

            var userEntity = new User
            {
                UserName = user.Username,
                Password = user.Password
            };

            var result = await userRepo.GetUser(userEntity);

            if (result != null)
            { await userRepo.Delete(result);
                return true;
            }
            return false;
        }


        public async Task<UserDTO> Login(LoginDTO user)
        {
            var userEntity = new User
            {
                UserName = user.Username,
                Password = user.Password
            };

            var result = await userRepo.GetUser(userEntity);

            if (result == null)
                return null;

            var token = GenerateToken(result.ToDto());

            var refreshToken = GenerateRefreshToken();

            //userRepo.Update
            //guardar el refresh Token

            UserDTO userDto = new UserDTO()
            {
                UserName = user.Username,
                Password = user.Password,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            };

            return userDto;
        }


        public async Task<bool> SignIn(UserDTO user)
        {
            //usar "sal" y un algoritmo de encriptado, nunca desencriptar las claves, correr al menos 1000 veces el encriptado
            //pasar a alguna codificacion para no depender de la maquina del pibe o y eso 
            //prior to digesting, perform string-to-byte sequence translation using a fixed encoding, preferably UTF-8.
            // By encoding our digested sequence of bytes in BASE64, we will make sure that the output byte sequence represents a valid, displayable, US-ASCII character string. 
            //So we will be able to safely translate the BASE64-encoded byte sequence into a character string specifying US-ASCII as the encoding.

            var exitinUser = userRepo.FindBy(x => x.UserName == user.UserName); /// probar si funciona, si lo hace no hay necesidad del método de abajo y se puede borrar.

            var existingUser = await userRepo.GetUserByUserName(user.UserName);
            if (existingUser != null)
                return false;

            await userRepo.Add(user.ToEntity());
            return true;
        }


        public async Task<UserDTO> RefreshTokens(string token, string refreshToken)
        {

            var principal = GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = await userRepo.GetRefreshTokenAsync(username);

            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var user = new UserDTO
            {
                UserName = principal.Claims.Where(x => x.Type == "sub").First().Value // extraigo el username de la claim, tendría que cambiar las claims despues para que una sea el nombre
            };

            var newJwtToken = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            newJwtToken.Claims.Where(x => x.Type == "edad");

            //DeleteRefreshToken(username, refreshToken); simanejara muchos refresh tokens debería borrar el anterior, como aca manejo uno solo lo piso
            await userRepo.SaveRefreshToken(username, newRefreshToken); //idem

            return new UserDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(newJwtToken),
                RefreshToken = newRefreshToken
            };

        }




        //este método se usa para extraer las claims del token vencido.
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Resources.Encription_Key)),
                ValidateLifetime = false //le digo que no verifique la fecha.
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private JwtSecurityToken GenerateToken(UserDTO user)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Resources.Encription_Key));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // cuando la base tenga mas datos importantes tipo version, edad , etc, eso se mete en una claim
                var claim =(new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                // podes agregar otras sub y claims, es un arreglo, pero es mejor crear una claim nueva y chau.
                new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,"sass"),
                new Claim(ClaimTypes.Role,"admin"),
                new Claim("edad", "user.edad")
                //new Claim(ClaimTypes.Name, "John") //there are some claim types that enable functionalities in.NET.
                                              //ClaimTypes.Name is the default claim type for the user’s name (User.Identity.Name).
                                              //Another example is ClaimTypes.Role that will be checked if you use the Roles property in an Authorize attribute (e.g. [Authorize(Roles="Administrator")]).
            });

                //var principal = new ClaimsPrincipal(claim); en algunos tutoriales usaban esto, pero no puedo usar claims asi y si lo cambio no las puedo asignar en el jwtsecurity token

                return new JwtSecurityToken(
                            issuer: Resources.Issuer,
                            audience: Resources.Audience,
                            claims: claim,
                            expires: DateTime.UtcNow.AddHours(Int32.Parse(Resources.Token_Duration)),
                            notBefore: DateTime.UtcNow, // a partir de cuando se puede usar el token
                            signingCredentials: cred
                                            );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
