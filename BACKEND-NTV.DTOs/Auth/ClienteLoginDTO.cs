using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DTOs.Auth
{
    public class ClienteLoginDTO
    {
        public int IdUsuario { get; set; }

        public string correo { get; set; }
        public string contrasena { get; set; }
    }
    

}
