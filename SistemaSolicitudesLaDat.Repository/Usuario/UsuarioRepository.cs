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
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                return await connection.QueryAsync<Usuario>(
                    "mostrar_usuarios",
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAllAsync: {ex.Message}");
                return Enumerable.Empty<Usuario>(); // Retorna lista vacía en caso de error
            }
        }

        public async Task<Usuario?> GetByIdAsync(string id)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("pI_id_usuario", id, DbType.String);

                return await connection.QuerySingleOrDefaultAsync<Usuario>(
                    "mostrar_usuario_por_id",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetByIdAsync: {ex.Message}");
                return null; // Retorna `null` en caso de error
            }
        }

        public async Task<int> InsertAsync(Usuario usuario)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error en InsertAsync: {ex.Message}");
                return 0; // Retorna `0` en caso de error
            }
        }

        public async Task<int> UpdateAsync(Usuario usuario)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("pI_id_usuario", usuario.Id_Usuario, DbType.String);
                parameters.Add("pI_nombre_usuario", usuario.Nombre_Usuario, DbType.String);
                parameters.Add("pI_nombre_completo", usuario.Nombre_Completo, DbType.String);
                parameters.Add("pI_correo_electronico", usuario.Correo_Electronico, DbType.String);
                parameters.Add("pI_estado", usuario.Estado.ToString(), DbType.String);
                parameters.Add("pS_resultado", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("actualizar_usuario_por_id", parameters, commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("pS_resultado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpdateAsync: {ex.Message}");
                return 0; // Retorna `0` en caso de error
            }
        }

        public async Task<int> DeleteAsync(string id_usuario)
        {
            try
            {
                using var connection = _dbConnectionFactory.CreateConnection();
                var parameters = new DynamicParameters();
                parameters.Add("pI_id_usuario", id_usuario);
                parameters.Add("pS_resultado", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync("eliminar_usuario_por_id", parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("pS_resultado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en DeleteAsync: {ex.Message}");
                return 0; // Retorna `0` en caso de error
            }
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosPaginadosAsync(int pagina, int tamanoPagina)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("pI_pagina", pagina);
            parameters.Add("pI_tamano_pagina", tamanoPagina);

            return await connection.QueryAsync<Usuario>(
                "mostrar_usuarios_paginado",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CuentaUsuariosAsync()
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            return await connection.ExecuteScalarAsync<int>(
                "cuenta_usuarios"
            );
        }

    }
}
