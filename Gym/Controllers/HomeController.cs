using System.Diagnostics;
using Gym.Models;
using Gym.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly IRepositorioHome repositorioHome;

        public HomeController(IRepositorioHome repositorioHome)
        {
            this.repositorioHome = repositorioHome;
        }


        public async Task<IActionResult> Index()
        {
            var model = await repositorioHome.ObtenerDatosDashPrincipal();
            return View(model);
        }


        public async Task<IActionResult> DetallesUsuario(int id)
        {
            var usuario = await repositorioHome.ObtenerInformacionUsuario(id);
            return View(usuario); 
        }
    }
}
