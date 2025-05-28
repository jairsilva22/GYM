using Dapper;
using Gym.Models;
using Org.BouncyCastle.Crypto.Generators;
using System.Data.SqlClient;

namespace Gym.Servicios
{
    public interface IRepositorioEntrenadores
    {
      
        Task<Entrenador> Crear(Entrenador entrenador);
        Task<Entrenador> Editar(Entrenador entrenador);
        Task<Entrenador> Eliminar(int id);
        Task<Entrenador> ObtenerPorId(int id);
        Task<IEnumerable<Entrenador>> Obtenertodos();
    }
    public class RepositorioEntrenadores : IRepositorioEntrenadores
    {
           private readonly string _connectionString;
        public RepositorioEntrenadores(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }



        public async Task<Entrenador> Crear(Entrenador entrenador)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "INSERT INTO Entrenadores (Nombre, Email,Telefono, PasswordHash) VALUES (@Nombre, @Email,@Telefono, @PasswordHash)";
            entrenador.PasswordHash = BCrypt.Net.BCrypt.HashPassword(entrenador.PasswordHash);
            connection.Execute(sql, entrenador);
            return entrenador;
        }


        public async Task<IEnumerable<Entrenador>> Obtenertodos()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Entrenador>("SELECT * FROM Entrenadores");
        }

        public async Task<Entrenador> Editar(Entrenador entrenador)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "UPDATE Entrenadores SET Nombre = @Nombre, Email = @Email, Telefono = @Telefono WHERE Id = @Id";
            connection.Execute(sql, entrenador);
            return entrenador;
        }


        public async Task<Entrenador> ObtenerPorId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Entrenador>("SELECT * FROM Entrenadores WHERE Id = @Id", new { Id = id });
        }   



        public async Task<Entrenador> Eliminar(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "DELETE FROM Entrenadores WHERE Id = @Id";
            connection.Execute(sql, new { Id = id });
            return null;
        }


        

    }
}
