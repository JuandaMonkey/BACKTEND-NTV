using Microsoft.AspNetCore.Mvc;
using BACKEND_NTV.DATA.Interfaces;
using BACKEND_NTV.DTOs.Auth;
using BACKEND_NTV.MODEL;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BACKEND_NTV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (loginDto == null || string.IsNullOrEmpty(loginDto.Correo) || string.IsNullOrEmpty(loginDto.Contrasena))
                    return BadRequest(new { mensaje = "Correo y contraseña son requeridos" });

                // ✅ 1. BUSCAR USUARIO (SEGURO - ANTI SQL INJECTION)
                var usuario = await _authRepository.ObtenerUsuarioPorCorreoAsync(loginDto.Correo);

                if (usuario == null)
                {
                    await _authRepository.RegistrarLogAsync(0, "LOGIN_FALLIDO_USUARIO_NO_EXISTE");
                    return Unauthorized(new { mensaje = "Credenciales inválidas" });
                }

                // ✅ 2. VERIFICAR CONTRASEÑA (SEGURO - BCRYPT)
                var contrasenaValida = await _authRepository.VerificarContrasenaAsync(loginDto.Contrasena, usuario.Contrasena);

                if (!contrasenaValida)
                {
                    await _authRepository.RegistrarLogAsync(usuario.IdUsuario, "LOGIN_FALLIDO_CONTRASENA_INCORRECTA");
                    return Unauthorized(new { mensaje = "Credenciales inválidas" });
                }

                // ✅ 3. GENERAR TOKEN JWT
                var token = GenerarTokenJwt(usuario);

                // ✅ 4. REGISTRAR LOG EXITOSO
                await _authRepository.RegistrarLogAsync(usuario.IdUsuario, "LOGIN_EXITOSO");

                // ✅ 5. REGRESAR TOKEN
                var respuesta = new LoginResponseDto
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    Correo = usuario.Correo,
                    Rol = "Usuario", // Como fk_idrol siempre es 2
                    Token = token,
                    Expiracion = DateTime.UtcNow.AddHours(2)
                };

                return Ok(respuesta);
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                    return BadRequest(new { mensaje = "Token no proporcionado" });

                // ✅ AGREGAR TOKEN A BLACKLIST (PARA QUE NO SIRVA AL COPIAR URL)
                var expiracion = DateTime.UtcNow.AddHours(2);
                var resultado = await _authRepository.AgregarTokenABlacklistAsync(token, expiracion);

                if (resultado)
                    return Ok(new { mensaje = "Sesión cerrada exitosamente" });
                else
                    return BadRequest(new { mensaje = "Error al cerrar sesión" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor" });
            }
        }

        private string GenerarTokenJwt(UsuarioModel usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, "Usuario"), // Como fk_idrol siempre es 2
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}