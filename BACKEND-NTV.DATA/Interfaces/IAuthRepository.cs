using BACKEND_NTV.MODEL;

namespace BACKEND_NTV.DATA.Interfaces
{
    public interface IAuthRepository
    {
        Task<UsuarioModel> ObtenerUsuarioPorCorreoAsync(string correo);
        Task<bool> VerificarContrasenaAsync(string contrasenaPlana, string contrasenaHash);
        Task<bool> RegistrarLogAsync(int usuarioId, string accion);
        Task<bool> ExisteTokenEnBlacklistAsync(string token);
        Task<bool> AgregarTokenABlacklistAsync(string token, DateTime expiracion);
    }
}