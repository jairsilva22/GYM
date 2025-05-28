using Gym.Models;
using Gym.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    //[Authorize]
    public class UsuariosController : Controller
    {
        private readonly IRepositorioUsuarios repositorioUsuarios;

        public UsuariosController(IRepositorioUsuarios repositorioUsuarios)
        {
            this.repositorioUsuarios = repositorioUsuarios;
        }


        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            var retorno= await repositorioUsuarios.Crear(usuario);

            if (retorno > 0)
            {
                ModelState.AddModelError("Correo", "El correo o teléfono ya existen");
                return View(usuario);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await repositorioUsuarios.ObtenerPorId(id);
            return View(usuario);
        }


        public async Task<IActionResult> Detalles(int id)
        {
            var usuario = await repositorioUsuarios.ObtenerPorId(id);
            return View(usuario);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await repositorioUsuarios.ObtenerPorId(id);
            return View(usuario);
        }

        [HttpPost]

        public async Task<IActionResult> EliminarUsuario(int id)
        {
            await repositorioUsuarios.Eliminar(id);
            return RedirectToAction("Index");
        }



        [HttpPost]

        public async Task<IActionResult> Editar(Usuario usuario)
        {
            await repositorioUsuarios.Editar(usuario);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await repositorioUsuarios.ObtenerTodos();
            return View(usuarios);
        }
    }
}
