using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DATA.Filters
{
    public class CortometrajeFilter
    {
        public string? Titulo { get; set; }

        public int? DuracionMinutos { get; set; }  

        public int? DuracionSegundos { get; set; }

        public int? Anio { get; set; }

        public bool? Estado { get; set; }

        public string? Categoria { get; set; }

        public int? Skip { get; set; } = 0;

        public int? Take { get; set; } = 10;
    }
}