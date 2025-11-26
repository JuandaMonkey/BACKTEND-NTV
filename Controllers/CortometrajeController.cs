using BACKEND_NTV.DATA.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BACKEND_NTV.DATA.Filters;

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

        [HttpGet("Listar-Cortometrajes")]
        public async Task<IActionResult> GetCortometrajes([FromQuery] CortometrajeFilter filters)
        {
            try
            {
                if (_cortometrajeRepository is null)
                    return StatusCode(500, "Repositorio no configurado.");

                var result = await _cortometrajeRepository.GetCortometrajes(filters);
                return Ok(new { 
                    data = result.Data, 
                    total = result.Total 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    data = new List<object>(),
                    total = 0,
                    error = ex.Message
                });
            }
        }
    }
}
