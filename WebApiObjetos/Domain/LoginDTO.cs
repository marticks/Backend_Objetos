using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiObjetos.Domain
{
    public class LoginDTO
    {
        [DataMember()]
        [Required(ErrorMessage = "UserName cannot be empty")]
        public string Username { get; set; }

        [DataMember()]
        [Required(ErrorMessage = "PassWord cannot be empty")]
        public string Password { get; set; }
    }
}
