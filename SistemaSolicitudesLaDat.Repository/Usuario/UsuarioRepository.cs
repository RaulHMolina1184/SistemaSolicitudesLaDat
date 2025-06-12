using Dapper;
using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Repository.Infrastructure;
using System.Data;

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
                var parametros = new DynamicParameters();

                parametros.Add("pI_nombre_usuario", usuario.NombreUsuario);
                parametros.Add("pI_nombre_completo", usuario.NombreCompleto);
                parametros.Add("pI_correo_electronico", usuario.CorreoElectronico);
                parametros.Add("pI_contrasenia_cifrada", usuario.ContraseniaCifrada, DbType.Binary);
                parametros.Add("pI_tag", usuario.TagAutenticacion, DbType.Binary);
                parametros.Add("pI_nonce", usuario.Nonce, DbType.Binary);
                parametros.Add("pI_estado", usuario.Estado.ToString());
                parametros.Add("pS_resultado", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("ingreso_nuevo_usuario", parametros, commandType: CommandType.StoredProcedure);

                return parametros.Get<int>("pS_resultado"); // 1 si éxito, 0 si error
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
