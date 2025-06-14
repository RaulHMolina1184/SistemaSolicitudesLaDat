using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Repository.Usuarios;
using SistemaSolicitudesLaDat.Service.Abstract;


namespace SistemaSolicitudesLaDat.Service.Usuarios
{
    public class UsuarioService : IUsuarioService 
    {

        private readonly UsuarioRepository _usuarioRepository;
        private readonly IBitacoraService _bitacoraService;

        public UsuarioService(UsuarioRepository usuarioRepository, IBitacoraService bitacoraService)
        {
            _usuarioRepository = usuarioRepository;
            _bitacoraService = bitacoraService;
        }

        public Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return _usuarioRepository.GetAllAsync();
        }

        public Task<Usuario?> GetByIdAsync(string id_usuario)
        {
            return _usuarioRepository.GetByIdAsync(id_usuario);
        }

        public async Task<int> InsertAsync(Usuario usuario, string idUsuarioEjecutor)
        {
            try
            {
                var resultado = await _usuarioRepository.InsertAsync(usuario);
                if (resultado == 1)
                {
                    await _bitacoraService.RegistrarAccionAsync(
                        idUsuarioEjecutor,
                        "Usuario creado",
                        new
                        {
                            usuario.Id_Usuario,
                            usuario.Nombre_Usuario,
                            usuario.Nombre_Completo,
                            usuario.Correo_Electronico,
                            Estado = usuario.Estado.ToString()
                        }
                    );
                }

                return resultado;
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync(idUsuarioEjecutor, ex.ToString());
                throw;
            }
        }

        public Task<int> UpdateAsync(Usuario usuario)
        {
            return _usuarioRepository.UpdateAsync(usuario);
        }

        public Task<int> DeleteAsync(string id_usuario)
        {
            return _usuarioRepository.DeleteAsync(id_usuario);
        }
    }

}
