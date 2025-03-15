using Gym.Models;
using Gym.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gym.Controllers
{
    public class PagosController : Controller
    {
        private readonly IRepositorioPagos repositorioPagos;
        private readonly IRepositorioUsuarios repositorioUsuarios;
        private readonly IRepositorioMensualidades repositorioMensualidades;

        public PagosController(IRepositorioPagos repositorioPagos, IRepositorioUsuarios repositorioUsuarios, IRepositorioMensualidades repositorioMensualidades)
        {
            this.repositorioPagos = repositorioPagos;
            this.repositorioUsuarios = repositorioUsuarios;
            this.repositorioMensualidades = repositorioMensualidades;
        }



        public async Task<IActionResult> Crear()
        {
            var modelo = new PagoViewModel();

            modelo.Usuarios = await ObtenerUsuarios();
            modelo.Mensualidades = await ObtenerMensualidades();

            return View(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Pago pago)
        {
            await repositorioPagos.Crear(pago);
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Index()
        {
            var pagos = await repositorioPagos.ObtenerPagos();
            return View(pagos);
        }

        public async Task<IEnumerable<MensualidadViewModel>> ObtenerMensualidades()
        {
            var mensualidades = await repositorioMensualidades.ObtenerTodos();

            return mensualidades.Select(m => new MensualidadViewModel
            {
                Value = m.Id.ToString(),
                Text = m.Tipo,
                Precio = m.Precio
            });
        }

        public async Task<IEnumerable<SelectListItem>> ObtenerUsuarios()
        {
            var usuarios = await repositorioUsuarios.ObtenerTodos();
            return usuarios.Select(u => new SelectListItem(u.Nombre, u.Id.ToString()));
        }


        public async Task<IActionResult> Editar(int id)
        {
            var pago = await repositorioPagos.ObtenerPagoId(id);
            if (pago == null)
            {
                return NotFound();
            }

            pago.Usuarios = await ObtenerUsuarios();
            pago.Mensualidades = await ObtenerMensualidades();

            return View(pago);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Pago pago)
        {
            await repositorioPagos.Editar(pago);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var pago = await repositorioPagos.ObtenerPagoId(id);
            if (pago == null)
            {
                return NotFound();
            }
            return View(pago);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarPago(int id)
        {
            await repositorioPagos.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
