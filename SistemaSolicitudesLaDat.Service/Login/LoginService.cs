using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Repository.Login;
using SistemaSolicitudesLaDat.Service.Encriptado;


namespace SistemaSolicitudesLaDat.Service.Login
{
    public class LoginService
    {
        private readonly LoginRepository _loginRepository;
        private readonly EncriptadoService _encriptadoService;

        // Constructor que recibe para obtener la cadena de conexión y el servicio de encriptación (key)
        public LoginService(LoginRepository loginRepository, EncriptadoService encriptadoService)
        {
            _loginRepository = loginRepository;
            _encriptadoService = encriptadoService;
        }
        // Método para validar login
        public Usuario? Login(string nombreUsuario, string inputContrasenia)
        {
            var usuario = _loginRepository.VerificarUsuario(nombreUsuario); // Llama sp hacía la base de datos

            // Si no existe o está bloqueado
            if (usuario.Encontrado != 1 || usuario.ContraseniaCifrada == null || usuario.Nonce == null || usuario.TagAutenticacion == null)
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
                _loginRepository.ReiniciarIntentos(nombreUsuario); // Resetear contador e intento
                return usuario;
            }
            else
            {
                _loginRepository.RegistrarIntentoFallido(nombreUsuario); // Incrementar contador y bloquear si corresponde
                return null;
            }
        }
    }
}
