using SistemaSolicitudesLaDat.Service.Abstract;
using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Repository.Usuarios;

namespace SistemaSolicitudesLaDat.Service.Usuarios
{
    public class UsuarioService : IUsuarioService 
    {

        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return _usuarioRepository.GetAllAsync();
        }

        public Task<Usuario?> GetByIdAsync(string id)
        {
            return _usuarioRepository.GetByIdAsync(id);
        }

        public Task<int> InsertAsync(Usuario usuario)
        {
            return _usuarioRepository.InsertAsync(usuario);
        }

        public Task<int> UpdateAsync(Usuario usuario)
        {
            return _usuarioRepository.UpdateAsync(usuario);
        }

        public Task<int> DeleteAsync(string id)
        {
            return _usuarioRepository.DeleteAsync(id);
        }
    }

}
