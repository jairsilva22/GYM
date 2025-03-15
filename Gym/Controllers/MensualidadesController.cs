using Gym.Models;
using Gym.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    public class MensualidadesController : Controller
    {
        private readonly IRepositorioMensualidades repositorioMensualidades;

        public MensualidadesController(IRepositorioMensualidades repositorioMensualidades)
        {
            this.repositorioMensualidades = repositorioMensualidades;
        }



        public  IActionResult Crear()
        {
            return View(); 
        }

        [HttpPost]

        public async Task<IActionResult> Crear(Mensualidades mensualidad)
        {
            await repositorioMensualidades.Crear(mensualidad);
            return RedirectToAction("Index");
        }

        

        public async Task<IActionResult> Index()
        {
            var mensualidad = await repositorioMensualidades.ObtenerTodos();
            return View(mensualidad);
        }



        public async Task<IActionResult> Editar(int id)
        {
            var mensualidad = await repositorioMensualidades.ObtenerPorId(id);
            return View(mensualidad);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Mensualidades mensualidad)
        {
            await repositorioMensualidades.Editar(mensualidad);
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Eliminar(int id)
        {
            var mensualidad = await repositorioMensualidades.ObtenerPorId(id);
            return View(mensualidad);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarMensualidad(int id)
        {
            await repositorioMensualidades.Eliminar(id);
            return RedirectToAction("Index");
        }


    }
}
