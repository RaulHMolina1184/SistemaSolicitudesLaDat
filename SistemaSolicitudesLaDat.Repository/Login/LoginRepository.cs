using Dapper;
using System.Data;
using SistemaSolicitudesLaDat.Entities.Usuarios;
using SistemaSolicitudesLaDat.Repository.Infrastructure;

namespace SistemaSolicitudesLaDat.Repository.Login
{
    public class LoginRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory; // Fábrica para crear conexiones a la base de datos
        public LoginRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public Usuario VerificarUsuario(string nombreUsuario) // Método para verificar las credenciales del usuario (entities)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("Nombre de usuario inválido", nameof(nombreUsuario));

            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                connection.Open();

                var parameters = new DynamicParameters();

                // Parámetro de entrada
                parameters.Add("@pI_nombre_usuario", nombreUsuario, DbType.String, ParameterDirection.Input);

                // Parámetros de salida
                parameters.Add("@pS_id_usuario", dbType: DbType.String, size: 10, direction: ParameterDirection.Output);
                parameters.Add("@pS_nombre_usuario", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                parameters.Add("@pS_nombre_completo", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                parameters.Add("@pS_correo_electronico", dbType: DbType.String, size: 100, direction: ParameterDirection.Output);
                parameters.Add("@pS_contrasenia_cifrada", dbType: DbType.Binary, size: 240, direction: ParameterDirection.Output);
                parameters.Add("@pS_tag", dbType: DbType.Binary, size: 16, direction: ParameterDirection.Output);
                parameters.Add("@pS_nonce", dbType: DbType.Binary, size: 12, direction: ParameterDirection.Output);
                parameters.Add("@pS_encontrado", dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Ejecutar el procedimiento almacenado
                connection.Execute("verificar_credencial", parameters, commandType: CommandType.StoredProcedure);

                // Verificar si se encontró el usuario y asgnar los valores de salida a entities
                return new Usuario
                {
                    Id_Usuario = parameters.Get<string>("@pS_id_usuario"),
                    Nombre_Usuario = parameters.Get<string>("@pS_nombre_usuario"),
                    Nombre_Completo = parameters.Get<string>("@pS_nombre_completo"),
                    Correo_Electronico = parameters.Get<string>("@pS_correo_electronico"),
                    ContraseniaCifrada = parameters.Get<byte[]>("@pS_contrasenia_cifrada"),
                    TagAutenticacion = parameters.Get<byte[]>("@pS_tag"),
                    Nonce = parameters.Get<byte[]>("@pS_nonce"),
                    Encontrado = parameters.Get<int>("@pS_encontrado")
                };
            }
        }

        public void RegistrarIntentoFallido(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("Nombre de usuario inválido", nameof(nombreUsuario));

            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@pI_nombreUsuario", nombreUsuario, DbType.String, ParameterDirection.Input);

                connection.Execute("registrar_intento_fallido", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void ReiniciarIntentos(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new ArgumentException("Nombre de usuario inválido", nameof(nombreUsuario));

            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                connection.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@pI_nombreUsuario", nombreUsuario, DbType.String, ParameterDirection.Input);

                connection.Execute("reiniciar_intentos", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
