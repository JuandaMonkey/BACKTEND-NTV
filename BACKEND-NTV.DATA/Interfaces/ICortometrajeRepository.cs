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
        public Task<CortometrajesResultDTO> GetCortometrajes(CortometrajeFilter filters);

        public Task<CortometrajeResultDTO> GetCortometrajeById(int idCortometraje);

        public Task<CortometrajeResultDTO> PostCortometraje(CortometrajeCreateDTO cortometraje);

        public Task<CortometrajeResultDTO> PutCortometraje(int idCortometraje, 
                                                     CortometrajeUpdateDTO cortometrajeUpdate,      
                                                     CortometrajeCategoriasUpdateDTO categoriaUpdate);

        public Task<CortometrajeResultDTO> DeleteCortometraje(int idCortometraje);
    }
}
