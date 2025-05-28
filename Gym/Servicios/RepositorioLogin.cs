using Dapper;
using Gym.Models;
using System.Data.SqlClient;

namespace Gym.Servicios
{
    public interface IRepositorioLogin
    {
        Entrenador Autenticar(string email, string password);
    }
    public class RepositorioLogin : IRepositorioLogin
    {
        private readonly string _connectionString;
        public RepositorioLogin(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Entrenador? Autenticar(string email, string password)
        {
            using var connection = new SqlConnection(_connectionString);

            string sql = "SELECT * FROM Entrenadores WHERE Email = @Email";
            var entrenador = connection.QueryFirstOrDefault<Entrenador>(sql, new { Email = email });

            // Si no encuentra el usuario o la contraseña es incorrecta
            if (entrenador == null || !BCrypt.Net.BCrypt.Verify(password, entrenador.PasswordHash))
            {
                return null; // Retorna null si la autenticación falla
            }

            return entrenador; // Retorna el usuario autenticado
        }

    }
}
