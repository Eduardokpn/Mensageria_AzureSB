using Dapper;
using MensageriaProject.Models;
using Microsoft.Data.SqlClient;

namespace MensageriaProject.Interfaces.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConexaoLocal");
        }

        public void Add(Usuario usuario)
        {
            using (var connnection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Usuario (Email, Senha) VALUES (@Email, @Senha)";

                connnection.Execute(sql,usuario);
            }

        }
    }
}
