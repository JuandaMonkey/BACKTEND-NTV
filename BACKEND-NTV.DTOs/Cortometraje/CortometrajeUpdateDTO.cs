using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DTOs.Cortometraje
{
    public class CortometrajeUpdateDTO
    {
        public string? Titulo { get; set; }

        public string? Autor { get; set; }

        public string? Descripcion { get; set; }

        public string? UrlVideo { get; set; }

        public string? UrlPortada { get; set; }

        public int? DuracionMinutos { get; set; }

        public int? DuracionSegundos { get; set; }

        public int? AnioLanzamiento { get; set; }

        public bool? Estado { get; set; }
    }
}
