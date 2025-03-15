using Gym.Models;
using Gym.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace Gym.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IRepositorioDashboard repositorioDashboard;

        public DashboardController(IRepositorioDashboard repositorioDashboard)
        {
            this.repositorioDashboard = repositorioDashboard;
        }
        public async Task<IActionResult> Index()
        {
            var model =  await repositorioDashboard.ObtenerDatosDashboard();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Filtrar([FromBody] FiltroFechaModel filtro)
        {
            var datosDashboard = await repositorioDashboard.ObtenerDatosDashboardFiltro(filtro.mes, filtro.anio);
            return Json(new
            {
                ingresosTotales = datosDashboard.IngresosTotales,
                cantidadPagos = datosDashboard.CantidadPagos,
                clientesActivos = datosDashboard.ClientesActivos,
                planMasVendido = datosDashboard.PlanMasVendido,
                pagos = datosDashboard.Pagos.Select(p => new {
                    miembro = p.Miembro,
                    fechaPago = p.FechaPago,
                    monto = p.Monto,
                    membresia = p.Membresia
                })
            });
        }

    }
}
