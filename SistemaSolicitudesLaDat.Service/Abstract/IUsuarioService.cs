using SistemaSolicitudesLaDat.Entities.Usuarios;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(string id);
        Task<int> InsertAsync(Usuario usuario, string idUsuarioEjecutor);
        Task<int> UpdateAsync(Usuario usuario, string idUsuarioEjecutor);  
        Task<int> DeleteAsync(string id_usuario, string idUsuarioEjecutor);
    }
}
