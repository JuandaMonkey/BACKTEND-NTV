using BACKEND_NTV.DATA.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND_NTV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        #region Repositories
        private readonly IAuthRepository? _authRepository;

        public AuthController(IAuthRepository authRepository) => _authRepository = authRepository;
        #endregion
    }
}
