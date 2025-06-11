using System.Data;

namespace SistemaSolicitudesLaDat.Repository.Infrastructure
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
