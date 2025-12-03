using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DTOs.Cortometraje
{
    public class CortometrajeCreateDTO
    {
        [Required(ErrorMessage = "El campo Título no se permite vacío.")]
        [StringLength(300, ErrorMessage = "El Título no puede superar los 300 caracteres.")]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "El campo Autor no se permite vacío.")]
        [StringLength(100, ErrorMessage = "El Autor no puede superar los 100 caracteres.")]
        public string? Autor { get; set; }

        [Required(ErrorMessage = "El campo Descripción no se permite vacío.")]
        [StringLength(500, ErrorMessage = "La Descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo UrlVideo no se permite vacío.")]
        [StringLength(500, ErrorMessage = "La URL del video no puede superar los 500 caracteres.")]
        [Url(ErrorMessage = "La URL del video no es válida.")]
        public string? UrlVideo { get; set; }

        [Required(ErrorMessage = "El campo UrlPortada no se permite vacío.")]
        [StringLength(500, ErrorMessage = "La URL de la portada no puede superar los 500 caracteres.")]
        [Url(ErrorMessage = "La URL de la portada no es válida.")]
        public string? UrlPortada { get; set; }

        [Required(ErrorMessage = "El campo Duración (minutos) no se permite vacío.")]
        [Range(0, 999, ErrorMessage = "Los minutos deben ser mayores o iguales a 0.")]
        public int DuracionMinutos { get; set; }

        [Required(ErrorMessage = "El campo Duración (segundos) no se permite vacío.")]
        [Range(0, 59, ErrorMessage = "Los segundos deben estar entre 0 y 59.")]
        public int DuracionSegundos { get; set; }

        [Required(ErrorMessage = "El campo Año de lanzamiento no se permite vacío.")]
        [Range(1900, 2100, ErrorMessage = "El Año de lanzamiento debe estar entre 1900 y 2100.")]
        public int AnioLanzamiento { get; set; }

        [Required(ErrorMessage = "El campo Estado es obligatorio.")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos una categoría.")]
        [MinLength(1, ErrorMessage = "Debe seleccionar al menos una categoría.")]
        public List<int> Categorias { get; set; } = new List<int>();

    }
}
