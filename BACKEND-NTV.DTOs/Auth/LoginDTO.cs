using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BACKEND_NTV.DTOs.Auth
{
    public class LoginDto
    {
        public string Correo { get; set; }
        public string Contrasena { get; set; }
    }

    public class LoginResponseDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }

}
