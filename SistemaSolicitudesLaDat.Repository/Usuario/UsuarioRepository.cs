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

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var usuarios = await connection.QueryAsync<Usuario>(
                    "mostrar_usuarios",
                    commandType: CommandType.StoredProcedure);

                return usuarios;
            }
        }

        public async Task<Usuario?> GetByIdAsync(string id)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("pI_id_usuario", id, System.Data.DbType.String);

                return await connection.QuerySingleOrDefaultAsync<Usuario>(
                    "mostrar_usuario_por_id",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure
                );
            }
        }

        public async Task<int> InsertAsync(Usuario usuario)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var parametros = new DynamicParameters();

                parametros.Add("pI_nombre_usuario", usuario.Nombre_Usuario);
                parametros.Add("pI_nombre_completo", usuario.Nombre_Completo);
                parametros.Add("pI_correo_electronico", usuario.Correo_Electronico);
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
                var parameters = new DynamicParameters();
                parameters.Add("pI_id_usuario", usuario.Id_Usuario, System.Data.DbType.String);
                parameters.Add("pI_nombre_usuario", usuario.Nombre_Usuario, System.Data.DbType.String);
                parameters.Add("pI_nombre_completo", usuario.Nombre_Completo, System.Data.DbType.String);
                parameters.Add("pI_correo_electronico", usuario.Correo_Electronico, System.Data.DbType.String);
                parameters.Add("pI_estado", usuario.Estado.ToString(), System.Data.DbType.String);
                parameters.Add("pS_resultado", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                await connection.ExecuteAsync("actualizar_usuario_por_id", parameters, commandType: System.Data.CommandType.StoredProcedure);

                return parameters.Get<int>("pS_resultado");
            }
        }

        public async Task<int> DeleteAsync(string id_usuario)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("pI_id_usuario", id_usuario);
            parameters.Add("pS_resultado", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("eliminar_usuario_por_id", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("pS_resultado");
        }
    }
}
