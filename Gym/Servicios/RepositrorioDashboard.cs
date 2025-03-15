using Dapper;
using Gym.Models;
using System.Data.SqlClient;

namespace Gym.Servicios
{

    public interface IRepositorioDashboard
    {
        Task<DashboardViewModel> ObtenerDatosDashboard();
        Task<DashboardViewModel> ObtenerDatosDashboardFiltro(int mes, int anio);
    }
    public class RepositrorioDashboard : IRepositorioDashboard
    {

        private readonly string _connectionString;

        public RepositrorioDashboard(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<DashboardViewModel> ObtenerDatosDashboard()
        {
            int mesActual = DateTime.Now.Month;  // Obtiene el mes actual (1-12)
            int anioActual = DateTime.Now.Year;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var ingresosTotales = await connection.ExecuteScalarAsync<decimal>("SELECT SUM(Monto) FROM Pagos");
                var cantidadPagos = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Pagos");
                var clientesActivos = await connection.ExecuteScalarAsync<int>("SELECT COUNT(DISTINCT c.Id) FROM Usuarios c INNER JOIN Pagos p ON c.Id = p.UsuarioId WHERE p.FechaPago IS NOT NULL AND p.FechaExpiracion >= GETDATE()");
                var planMasVendido = await connection.ExecuteScalarAsync<string>("SELECT TOP 1 Tipo FROM Pagos inner join Mensualidades on Pagos.MensualidadId = Mensualidades.Id GROUP BY Tipo ORDER BY COUNT(*) DESC");
                var ingresosMensuales = await connection.QueryAsync<IngresoMensual>(
                                                            @"   select  MONTH(FechaPago) as Mes,sum(monto) as Monto 
	                                                                       from Pagos
	                                                                       group by FechaPago
	                                                                       ");
                var pagos = await connection.QueryAsync<PagoViewModelIndex>("SELECT Pagos.Id, " +
                                                            "u.Nombre as Miembro, " +
                                                            "m.Tipo as Membresia, " +
                                                            "Pagos.Monto, " +
                                                            "Pagos.FechaPago, " +
                                                            "Pagos.FechaExpiracion " +
                                                            "FROM Pagos " +
                                                            "INNER JOIN Mensualidades m ON m.Id = Pagos.MensualidadId " +
                                                            "INNER JOIN Usuarios u ON u.Id = Pagos.UsuarioId");


                return new DashboardViewModel
                {
                    IngresosTotales = ingresosTotales,
                    CantidadPagos = cantidadPagos,
                    ClientesActivos = clientesActivos,
                    PlanMasVendido = planMasVendido,
                    IngresosMensuales = ingresosMensuales.ToList(),
                    Pagos = pagos.ToList()
                };
            }
        }

        public async Task<DashboardViewModel> ObtenerDatosDashboardFiltro(int mes, int anio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var ingresosTotales = await connection.ExecuteScalarAsync<decimal>(
                    @"SELECT SUM(Monto) 
              FROM Pagos 
              WHERE MONTH(FechaPago) = @Mes AND YEAR(FechaPago) = @Anio",
                    new { Mes = mes, Anio = anio });

                var cantidadPagos = await connection.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(*) 
              FROM Pagos 
              WHERE MONTH(FechaPago) = @Mes AND YEAR(FechaPago) = @Anio",
                    new { Mes = mes, Anio = anio });

                var clientesActivos = await connection.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(DISTINCT c.Id) 
              FROM Usuarios c 
              INNER JOIN Pagos p ON c.Id = p.UsuarioId 
              WHERE p.FechaPago IS NOT NULL 
              AND MONTH(p.FechaPago) = @Mes AND YEAR(p.FechaPago) = @Anio
              AND p.FechaExpiracion >= GETDATE()",
                    new { Mes = mes, Anio = anio });

                var planMasVendido = await connection.ExecuteScalarAsync<string>(
                    @"SELECT TOP 1 m.Tipo 
              FROM Pagos 
              INNER JOIN Mensualidades m ON Pagos.MensualidadId = m.Id 
              WHERE MONTH(Pagos.FechaPago) = @Mes AND YEAR(Pagos.FechaPago) = @Anio
              GROUP BY m.Tipo 
              ORDER BY COUNT(*) DESC",
                    new { Mes = mes, Anio = anio });

                var ingresosMensuales = await connection.QueryAsync<IngresoMensual>(
                    @"SELECT MONTH(FechaPago) as Mes, SUM(Monto) as Monto 
              FROM Pagos 
              WHERE YEAR(FechaPago) = @Anio
              GROUP BY MONTH(FechaPago)",
                    new { Anio = anio });

                var pagos = await connection.QueryAsync<PagoViewModelIndex>(
                    @"SELECT Pagos.Id, 
                     u.Nombre AS Miembro, 
                     m.Tipo AS Membresia, 
                     Pagos.Monto, 
                     Pagos.FechaPago, 
                     Pagos.FechaExpiracion 
              FROM Pagos 
              INNER JOIN Mensualidades m ON m.Id = Pagos.MensualidadId 
              INNER JOIN Usuarios u ON u.Id = Pagos.UsuarioId
              WHERE MONTH(Pagos.FechaPago) = @Mes AND YEAR(Pagos.FechaPago) = @Anio",
                    new { Mes = mes, Anio = anio });

                return new DashboardViewModel
                {
                    IngresosTotales = ingresosTotales,
                    CantidadPagos = cantidadPagos,
                    ClientesActivos = clientesActivos,
                    PlanMasVendido = planMasVendido,
                    IngresosMensuales = ingresosMensuales.ToList(),
                    Pagos = pagos.ToList()
                };
            }
        }



    }
}
