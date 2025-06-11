using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace SistemaSolicitudesLaDat.Repository.Infrastructure
{
    public class DbConnectionFactory : IDbConnectionFactory
    {

        private readonly IConfiguration _configuration;
        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            return new MySqlConnection(connectionString);
        }
    }
}