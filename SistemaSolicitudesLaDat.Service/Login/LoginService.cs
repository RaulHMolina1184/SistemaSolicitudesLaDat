using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Repository.Login;
using SistemaSolicitudesLaDat.Service.Encriptado;

namespace SistemaSolicitudesLaDat.Service.Login
{
    public class LoginService
    {
        private readonly LoginRepository _loginRepository;
        private readonly EncriptadoService _encriptadoService;

        public LoginService(LoginRepository loginRepository, EncriptadoService encriptadoService)
        {
            _loginRepository = loginRepository;
            _encriptadoService = encriptadoService;
        }

        public Usuario? Login(string nombreUsuario, string inputContrasenia)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(inputContrasenia))
                    throw new ArgumentException("El nombre de usuario o la contraseña no pueden estar vacíos.");

                var usuario = _loginRepository.VerificarUsuario(nombreUsuario);

                // Si no existe o está bloqueado
                if (usuario == null || usuario.Encontrado != 1 || usuario.ContraseniaCifrada == null || usuario.Nonce == null || usuario.TagAutenticacion == null)
                {
                    return null;
                }

                // Verificar contraseña
                bool esValida = _encriptadoService.VerificarContrasenia(
                    usuario.ContraseniaCifrada,
                    usuario.TagAutenticacion,
                    usuario.Nonce,
                    inputContrasenia
                );

                if (esValida)
                {
                    _loginRepository.ReiniciarIntentos(nombreUsuario); // Resetear contador de intentos
                    return usuario;
                }
                else
                {
                    _loginRepository.RegistrarIntentoFallido(nombreUsuario); // Incrementar contador y bloquear si corresponde
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Login: {ex.Message}");
                return null;
            }
        }
    }
}
