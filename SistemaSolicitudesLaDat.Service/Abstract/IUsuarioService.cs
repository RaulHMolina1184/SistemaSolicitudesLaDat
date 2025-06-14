using SistemaSolicitudesLaDat.Entities.Usuarios;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(string id);
        Task<int> InsertAsync(Usuario usuario, string idUsuarioEjecutor);
        Task<int> UpdateAsync(Usuario usuario);
        Task<int> DeleteAsync(string id);
    }
}
