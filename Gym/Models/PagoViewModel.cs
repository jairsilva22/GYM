using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace Gym.Models
{
    public class PagoViewModel : Pago
    {
   
        public string Miembro { get; set; }  // Nombre del usuario actual
        public string Membresia { get; set; } // Tipo de membresía actual
       

        public IEnumerable<SelectListItem> Usuarios { get; set; }
        public IEnumerable<MensualidadViewModel> Mensualidades { get; set; }
    }
}
