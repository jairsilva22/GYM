using Dapper;
using Gym.Models;
using System.Data.SqlClient;

namespace Gym.Servicios
{
    public interface IRepositorioMensualidades
    {
        Task Crear(Mensualidades mensualidad);
        Task Editar(Mensualidades mensualidad);
        Task Eliminar(int id);
        Task<Mensualidades> ObtenerPorId(int id);
        Task<IEnumerable<Mensualidades>> ObtenerTodos();
    }
    public class RepositorioMensualidades : IRepositorioMensualidades
    {
        private readonly string _connectionString;  
        public RepositorioMensualidades(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public async Task<IEnumerable<Mensualidades>> ObtenerTodos()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Mensualidades>("SELECT * FROM Mensualidades");

        }

        public async Task Crear(Mensualidades mensualidad)
        {
            using var connection = new SqlConnection(_connectionString);
                await connection.ExecuteAsync(
                    "INSERT INTO Mensualidades (Tipo, Precio, Duracion) VALUES (@Tipo, @Precio, @Duracion)",
                    new { mensualidad.Tipo, mensualidad.Precio, mensualidad.Duracion });
        }

        public async Task<Mensualidades> ObtenerPorId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Mensualidades>("SELECT * FROM Mensualidades WHERE Id = @id", new { id });
        }

        public async Task Editar(Mensualidades mensualidad)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE Mensualidades SET Tipo = @Tipo, Precio = @Precio, Duracion = @Duracion WHERE Id = @Id",
                new { mensualidad.Tipo, mensualidad.Precio, mensualidad.Duracion, mensualidad.Id });
        }

        public async Task Eliminar(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Mensualidades WHERE Id = @id", new { id });
        }


    }
}
