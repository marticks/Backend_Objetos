using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiObjetos.Domain
{
    [DataContract()]
    public class UserDTO
    {
        [DataMember()]
        public int Id { get; set; }
        [DataMember()]
        public string UserName { get; set; }
        [DataMember()]
        public string Password { get; set; }
    }
}
