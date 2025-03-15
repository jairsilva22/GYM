using Dapper;
using Gym.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Gym.Servicios
{
    public interface IRepositorioPagos
    {
        Task<Pago> Crear(Pago pago);
        Task<Pago> Editar(Pago pago);
        Task<Pago> Eliminar(int id);
        Task<PagoViewModel> ObtenerPagoId(int id);
        Task<IEnumerable<PagoViewModelIndex>> ObtenerPagos();
    }
    public class RepositorioPagos : IRepositorioPagos
    {
        private readonly string connectionString;

        public RepositorioPagos(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // funcion que va tener la informacion de la cuenta 


        public async Task<IEnumerable<PagoViewModelIndex>> ObtenerPagos()
        {
            using var connection = new SqlConnection(connectionString);
           
            return await connection.QueryAsync<PagoViewModelIndex>("select Pagos.Id as Id,u.Nombre as Miembro,m.Tipo as Membresia,Pagos.Monto as Monto,FechaPago,FechaExpiracion" +
                "                                    from Pagos " +
                "                                    inner join Mensualidades m on m.id = Pagos.MensualidadId" +
                "                                    inner join Usuarios u on u.Id = Pagos.UsuarioId");
            
        }


        public async Task<Pago> Crear(Pago pago)
        {
            using var connection = new SqlConnection(connectionString);
            var result = await connection.QuerySingleAsync<Pago>(
                @"INSERT INTO Pagos (UsuarioId, MensualidadId, Monto, FechaPago, FechaExpiracion)
                VALUES (@UsuarioId, @MensualidadId, @Monto, @FechaPago, @FechaExpiracion)
                SELECT SCOPE_IDENTITY()",
                pago
            );
            pago.Id = result.Id;
            return pago;
        }

        public async Task<PagoViewModel> ObtenerPagoId(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<PagoViewModel>(
                                    @"SELECT Pagos.Id, 
                                     Pagos.UsuarioId, 
                                     Pagos.MensualidadId, 
                                     u.Nombre as Miembro, 
                                     m.Tipo as Membresia, 
                                     Pagos.Monto, 
                                     Pagos.FechaPago, 
                                     Pagos.FechaExpiracion
                                      FROM Pagos
                                      INNER JOIN Mensualidades m ON m.Id = Pagos.MensualidadId
                                      INNER JOIN Usuarios u ON u.Id = Pagos.UsuarioId
                                      WHERE Pagos.Id = @id",
                                        new { id }
            );
        }
        public async Task<Pago> Editar(Pago pago)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE Pagos SET UsuarioId = @UsuarioId, MensualidadId = @MensualidadId, Monto = @Monto, FechaPago = @FechaPago, FechaExpiracion = @FechaExpiracion
                WHERE Id = @Id",
                pago
            );
            return pago;
        }


        public async Task<Pago> Eliminar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE FROM Pagos WHERE Id = @id", new { id });
            return null;
        }
    }
}
