using BACKEND_NTV.DTOs.Usuario;
using BACKEND_NTV.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_NTV.DATA.Interfaces
{
    public interface IClienteRepository
    {
        Task<int> CrearClienteAsync(UsuarioCreateDTO dto);
    }
}
