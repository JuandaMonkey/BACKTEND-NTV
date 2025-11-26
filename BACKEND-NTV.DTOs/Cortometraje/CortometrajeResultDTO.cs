using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DTOs.Cortometraje
{
    public class CortometrajeResultDTO
    {
        public IEnumerable<CortometrajeDTO> Data { get; set; } = new List<CortometrajeDTO>();
        public int Total { get; set; }
    }
}
