using Dapper;
using Gym.Models;
using Gym.Models.DashPrincipal;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Gym.Servicios
{
    public interface IRepositorioHome
    {
        Task<DashPrincipalViewModel> ObtenerDatosDashPrincipal();
        Task<UsuarioViewModel> ObtenerInformacionUsuario(int usuarioId);
    }
    public class RepositorioHome  : IRepositorioHome
    {
        private readonly string _connectionString;

        public RepositorioHome(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<DashPrincipalViewModel> ObtenerDatosDashPrincipal()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Obtener cantidad de pagos realizados en el mes y año actual
                var mesActual = DateTime.Now.Month;
                var anioActual = DateTime.Now.Year;

                var pagados = await connection.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(*) 
                              FROM Pagos 
                              WHERE MONTH(FechaPago) = @Mes 
                              AND YEAR(FechaPago) = @Anio",
                    new { Mes = mesActual, Anio = anioActual });

                // Usuarios activos: su último pago aún no expira
                var activos = await connection.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(*) 
              FROM (
                  SELECT UsuarioId, MAX(FechaExpiracion) AS UltimaFecha 
                  FROM Pagos 
                  GROUP BY UsuarioId
              ) AS UltimosPagos
              WHERE UltimaFecha >= GETDATE()");

                // Usuarios vencidos: su último pago ya expiró
                var vencidos = await connection.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(*) 
              FROM (
                  SELECT UsuarioId, MAX(FechaExpiracion) AS UltimaFecha 
                  FROM Pagos 
                  GROUP BY UsuarioId
              ) AS UltimosPagos
              WHERE UltimaFecha < GETDATE()");

                // Por vencer en los próximos 7 días
                var porVencer = await connection.ExecuteScalarAsync<int>(
                    @"SELECT COUNT(*) 
              FROM (
                  SELECT UsuarioId, MAX(FechaExpiracion) AS UltimaFecha 
                  FROM Pagos 
                  GROUP BY UsuarioId
              ) AS UltimosPagos
              WHERE UltimaFecha >= GETDATE()
              AND DATEDIFF(DAY, GETDATE(), UltimaFecha) <= 7");

                // Detalles de pagos activos
                var pagosActivos = await connection.QueryAsync<PagosDashPrincipal>(
                    @"SELECT p.Id, u.Id AS UsuarioId, u.Nombre, m.Tipo AS Membresia,
                     'Activo' AS Status,
                     CAST(DATEDIFF(DAY, GETDATE(), p.FechaExpiracion) AS VARCHAR) + ' días' AS diasRestantes,
                     p.FechaPago AS UltimoPago
              FROM Pagos p
              INNER JOIN Usuarios u ON p.UsuarioId = u.Id
              INNER JOIN Mensualidades m ON p.MensualidadId = m.Id
              WHERE p.FechaExpiracion >= GETDATE()
              AND p.FechaExpiracion = (
                  SELECT MAX(FechaExpiracion)
                  FROM Pagos p2
                  WHERE p2.UsuarioId = p.UsuarioId
              )
              ORDER BY p.FechaExpiracion DESC");

                // Detalles de pagos vencidos
                var pagosVencidos = await connection.QueryAsync<PagosVencidos>(
                    @"SELECT p.Id, u.Nombre AS Miembro, p.FechaExpiracion,
                     DATEDIFF(DAY, p.FechaExpiracion, GETDATE()) AS ultimoPago
              FROM Pagos p
              INNER JOIN Usuarios u ON p.UsuarioId = u.Id
              WHERE p.FechaExpiracion < GETDATE()
              AND p.FechaExpiracion = (
                  SELECT MAX(FechaExpiracion)
                  FROM Pagos
                  WHERE UsuarioId = p.UsuarioId
              )");

                // Detalles de pagos por vencer
                var proximosVencer = await connection.QueryAsync<PagosPorVencer>(
                    @"SELECT p.Id, u.Nombre AS Miembro, p.FechaExpiracion,
                     DATEDIFF(DAY, GETDATE(), p.FechaExpiracion) AS diasRestantes
              FROM Pagos p
              INNER JOIN Usuarios u ON p.UsuarioId = u.Id
              WHERE p.FechaExpiracion BETWEEN GETDATE() AND DATEADD(DAY, 7, GETDATE())
              AND p.FechaExpiracion = (
                  SELECT MAX(FechaExpiracion)
                  FROM Pagos
                  WHERE UsuarioId = p.UsuarioId
              )");

                return new DashPrincipalViewModel
                {
                    Pagados = pagados,
                    Activos = activos,
                    Vencidos = vencidos,
                    porVencer = porVencer,
                    Pagos = pagosActivos,
                    pagosVencidos = pagosVencidos,
                    pagosPorVencer = proximosVencer,
                };
            }
        }



        public async Task<UsuarioViewModel> ObtenerInformacionUsuario(int usuarioId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"SELECT u.Id, u.Nombre, u.Correo, u.Telefono, 
		                               COUNT(p.Id) AS MesesInscrito,
		                               MAX(p.FechaPago) AS UltimoPago,
		                               MIN(p.FechaPago) AS FechaInscripcion
	                            FROM Usuarios u
	                            LEFT JOIN Pagos p ON u.Id = p.UsuarioId
	                            where p.UsuarioId = 7
	                            GROUP BY u.Id, u.Nombre, u.Correo, u.Telefono
	                            ORDER BY u.Nombre";

                var result = await connection.QueryFirstOrDefaultAsync<UsuarioViewModel>(query, new { UsuarioId = usuarioId });

                return result;
            }
        }

    }
}
