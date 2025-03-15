using Dapper;
using Gym.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace Gym.Servicios
{
    public interface IRepositorioUsuarios
    {
   
        Task<int> Crear(Usuario usuario);
        Task<Usuario> Editar(Usuario usuario);
        Task Eliminar(int id);
        Task<Usuario> ObtenerPorId(int id);
        Task<IEnumerable<Usuario>> ObtenerTodos();
      
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {

        private readonly string _connectionString;

        public RepositorioUsuarios(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<Usuario> ObtenerPorId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            return await connection.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM Usuarios WHERE Id = @id", new { id });
            
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodos()
        {
            using var connection = new SqlConnection(_connectionString);

            return await connection.QueryAsync<Usuario>("SELECT * FROM Usuarios");
            
        }



    


        public async Task<int> Crear(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);

            // Verificar si el correo o teléfono ya existen
            var existe = await connection.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Usuarios WHERE Correo = @Correo OR Telefono = @Telefono",
                new { usuario.Correo, usuario.Telefono });

            if (existe > 0)
            {
                return 1;
            }

            // Insertar el usuario si no existe
            await connection.ExecuteAsync(
                "INSERT INTO Usuarios (Nombre, Correo, Telefono, FechaAlta) VALUES (@Nombre, @Correo, @Telefono, @FechaAlta)",
                usuario);

            return 0;
        }   



        public async Task<Usuario> Editar(Usuario usuario)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "UPDATE Usuarios SET Nombre = @Nombre, Correo = @Correo, Telefono = @Telefono WHERE Id = @Id",
                usuario);
            return usuario;
        }

        public async Task Eliminar(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync("DELETE FROM Usuarios WHERE Id = @id", new { id });
        }
    }
}
