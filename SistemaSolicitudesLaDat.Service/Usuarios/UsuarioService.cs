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

        public async Task<int> UpdateAsync(Usuario usuario, string idUsuarioEjecutor)
        {
            try
            {
                var resultado = await _usuarioRepository.UpdateAsync(usuario);
                if (resultado == 1)
                {
                    await _bitacoraService.RegistrarAccionAsync(
                        idUsuarioEjecutor,
                        "Usuario actualizado",
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

        public async Task<int> DeleteAsync(string id_usuario, string idUsuarioEjecutor)
        {
            try
            {
                // 1. Obtener los datos actuales del usuario antes de eliminar
                var usuarioExistente = await _usuarioRepository.GetByIdAsync(id_usuario);

                if (usuarioExistente == null)
                {
                    // Puedes registrar un intento de eliminación fallido si lo deseas
                    return 0; // No existe, no hay nada que eliminar
                }

                // 2. Eliminar
                var resultado = await _usuarioRepository.DeleteAsync(id_usuario);

                // 3. Registrar en bitácora solo si fue exitoso
                if (resultado == 1)
                {
                    await _bitacoraService.RegistrarAccionAsync(
                        idUsuarioEjecutor,
                        "Usuario eliminado",
                        new
                        {
                            usuarioExistente.Id_Usuario,
                            usuarioExistente.Nombre_Usuario,
                            usuarioExistente.Nombre_Completo,
                            usuarioExistente.Correo_Electronico,
                            usuarioExistente.Estado
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
    }
}
