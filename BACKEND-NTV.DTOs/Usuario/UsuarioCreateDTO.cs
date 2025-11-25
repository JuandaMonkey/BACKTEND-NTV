using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DTOs.Usuario
{
    public class UsuarioCreateDTO
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 20 caracteres.")]
        [RegularExpression(@"^[A-Za-z0-9]+$",
        ErrorMessage = "El nombre solo puede contener letras y números, sin espacios ni caracteres especiales.")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
        [StringLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres.")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(10, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 10 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,10}$",
        ErrorMessage = "La contraseña debe tener al menos una mayúscula, un número, no contener espacios ni caracteres especiales.")]
        public string? Contrasena { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Rol no válido.")]
        public int Fk_IdRol { get; set; }
    }
}
