using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiObjetos.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "UserName cannot be empty")] // si esta vacio hace fallar el modelstate
        public string UserName { get; set; }

        [Required()]
        public string Password { get; set; }

        public string Email { get; set; }

    }
}
