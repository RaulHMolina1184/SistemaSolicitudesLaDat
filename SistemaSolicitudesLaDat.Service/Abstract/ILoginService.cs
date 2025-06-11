using SistemaSolicitudesLaDat.Entities.Usuarios;

namespace SistemaSolicitudesLaDat.Service.Abstract
{
    public interface ILoginService
    {   
        Task<Usuario?> VerificarCredencialAsync(string nombreUsuario);
    }
    
}
