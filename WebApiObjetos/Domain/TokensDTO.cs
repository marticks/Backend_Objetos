using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiObjetos.Domain
{
    [DataContract()]
    public class TokensDTO
    {
        [DataMember()]
        public string Token { get; set; }
        [DataMember()]
        public string RefreshToken { get; set; }
    }
}
