using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BACKEND_NTV.DATA.Filters;
using BACKEND_NTV.DTOs.Cortometraje;

namespace BACKEND_NTV.DATA.Interfaces
{
    public interface ICortometrajeRepository
    {
        public Task<CortometrajeResultDTO> GetCortometrajes(CortometrajeFilter filters);
    }
}
