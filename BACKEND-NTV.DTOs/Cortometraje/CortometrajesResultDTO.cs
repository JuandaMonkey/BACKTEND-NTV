using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DTOs.Cortometraje
{
    public class CortometrajesResultDTO
    {
        public bool Exito { get; set; }

        public string? Mensaje { get; set; }

        public IEnumerable<CortometrajeDTO> Data { get; set; } = new List<CortometrajeDTO>();

        public int Total { get; set; }
    }
}
