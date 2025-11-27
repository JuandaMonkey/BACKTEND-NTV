using BACKEND_NTV.DATA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BACKEND_NTV.DATA.Filters;
using BACKEND_NTV.DTOs.Cortometraje;

namespace BACKEND_NTV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CortometrajeController : Controller
    {
        #region Repositories
        private readonly ICortometrajeRepository? _cortometrajeRepository;

        public CortometrajeController(ICortometrajeRepository cortometrajeRepository) => _cortometrajeRepository = cortometrajeRepository;
        #endregion

        #region GetCortometrajes
        [HttpGet("Listar-Cortometrajes")]
        public async Task<IActionResult> GetCortometrajes([FromQuery] CortometrajeFilter filters)
        {
            if (_cortometrajeRepository is null)
                return StatusCode(500, "Repositorio no configurado.");

            var result = await _cortometrajeRepository.GetCortometrajes(filters);
            if (!result.Exito)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion

        #region GetCortometrajeById
        [HttpGet("Obtener-Cortometraje-Por-Id")]
        public async Task<IActionResult> GetCortometrajes([FromQuery] int idCortometraje)
        {
            if (_cortometrajeRepository is null)
                return StatusCode(500, new { Exito = false, Mensaje = "Repositorio no configurado.", Data = (CortometrajeDTO?)null });

            var result = await _cortometrajeRepository.GetCortometrajeById(idCortometraje);

            if (!result.Exito)
                return NotFound(result);

            return Ok(result);
        }
        #endregion

        #region PostCortometraje
        [HttpPost("Agregar-Cortometraje")]
        public async Task<IActionResult> PostCortometraje([FromBody] CortometrajeCreateDTO cortometraje)
        {
            if (_cortometrajeRepository is null)
                return StatusCode(500, new { Exito = false, Mensaje = "Repositorio no configurado.", Data = (CortometrajeDTO?)null });

            var result = await _cortometrajeRepository.PostCortometraje(cortometraje);

            if (!result.Exito)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion

        #region PutCortometraje
        [HttpPut("Editar-Cortometraje")]
        public async Task<IActionResult> PutCortometraje(
            [FromQuery] int idCortometraje,
            [FromBody] CortometrajeUpdateRequest request)
        {
            if (_cortometrajeRepository is null)
                return StatusCode(500, new { Exito = false, Mensaje = "Repositorio no configurado.", Data = (CortometrajeDTO?)null });

            var result = await _cortometrajeRepository.PutCortometraje(
                idCortometraje,
                request.Cortometraje,
                request.Categorias
            );

            if (!result.Exito)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion

        #region DeleteCortometraje
        [HttpDelete("Eliminar-Cortometraje")]
        public async Task<IActionResult> DeleteCortometraje([FromQuery] int idCortometraje)
        {
            if (_cortometrajeRepository is null)
                return StatusCode(500, new { Exito = false, Mensaje = "Repositorio no configurado.", Data = (CortometrajeDTO ?)null });

            var result = await _cortometrajeRepository.DeleteCortometraje(idCortometraje);

            if (!result.Exito)
                return BadRequest(result);

            return Ok(result);
        }
        #endregion
    }
}
