
using BACKEND_NTV.DATA.Interfaces;
using BACKEND_NTV.DTOs.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND_NTV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _repo;

        public ClienteController(IClienteRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _repo.CrearClienteAsync(dto);

            return resultado switch
            {
                -1 => Conflict(new { mensaje = "El correo ya está registrado." }),
                -2 => StatusCode(500, new { mensaje = "Error interno al crear el usuario." }),
                _ => Ok(new
                {
                    mensaje = "Usuario creado correctamente",
                    idusuario = resultado
                })
            };
        }
    }
}

