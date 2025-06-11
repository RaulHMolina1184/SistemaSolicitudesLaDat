using Dapper;
using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Repository.Infrastructure;

namespace SistemaSolicitudesLaDat.Repository.Usuarios
{
    public class UsuarioRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UsuarioRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Entities.Usuarios.Usuario>> GetAllAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<Usuario>("SELECT PersonaID, Nombre, Tipo, Gender, Password FROM Persona");
            }
        }

        public async Task<Usuario?> GetByIdAsync(string id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<Usuario>("SELECT PersonaID, Nombre, Tipo, Gender, Password FROM Persona WHERE PersonaID = @Id", new { Id = id });
            }
        }

        public async Task<int> InsertAsync(Usuario usuario)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var sql = "INSERT INTO Persona (PersonaID, Nombre, Tipo, Gender, Password) VALUES (@PersonaID, @Nombre, @Tipo, @Gender, @Password)";
                return await connection.ExecuteAsync(sql, usuario);
            }
        }

        public async Task<int> UpdateAsync(Usuario usuario)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var sql = "UPDATE Persona SET Nombre = @Nombre, Tipo = @Tipo, Gender = @Gender, Password = @Password WHERE PersonaID = @PersonaID";
                return await connection.ExecuteAsync(sql, usuario);
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var sql = "DELETE FROM Persona WHERE PersonaID = @Id";
                return await connection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
