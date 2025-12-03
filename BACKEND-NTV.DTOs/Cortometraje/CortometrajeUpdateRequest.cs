using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DTOs.Cortometraje
{
    public class CortometrajeUpdateRequest
    {
        public CortometrajeUpdateDTO Cortometraje { get; set; } = new();

        public CortometrajeCategoriasUpdateDTO Categorias { get; set; } = new();
    }
}
