using Gym.Models;
using Gym.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Gym.Controllers
{
    public class EntrenadoresController : Controller
    {
        private readonly IRepositorioEntrenadores repositorioEntrenadores;

        public EntrenadoresController(IRepositorioEntrenadores repositorioEntrenadores)
        {
            this.repositorioEntrenadores = repositorioEntrenadores;
        }
        public async Task<IActionResult> Index()
        {
            var entrenadores = await  repositorioEntrenadores.Obtenertodos();
            return View(entrenadores);
            
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Entrenador entrenador)
        {

            await repositorioEntrenadores.Crear(entrenador);
            return RedirectToAction("Index");

        }



        public async Task<IActionResult> Editar(int id)
        {
            var entrenador = await repositorioEntrenadores.ObtenerPorId(id);
            return View(entrenador);
        }



        [HttpPost]

        public async Task<IActionResult> Editar(Entrenador entrenador)
        {
            await repositorioEntrenadores.Editar(entrenador);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Eliminar(int id)
        {
            var entrenador = await repositorioEntrenadores.ObtenerPorId(id);
            return View(entrenador);
        }


        [HttpPost]
        public async Task<IActionResult> Eliminar(Entrenador entrenador)
        {
            await repositorioEntrenadores.Eliminar(entrenador.Id);
            return RedirectToAction("Index");
        }
    }
}
