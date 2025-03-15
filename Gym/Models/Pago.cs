using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class Pago
    {
        public int Id { get; set; }

        [DisplayName("Nombre del miembro")]
        public int UsuarioId { get; set; }


        [DisplayName("Tipo de mensualidad")]
        public int MensualidadId { get; set; }
        public decimal Monto { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime FechaExpiracion { get; set; } = DateTime.Today.AddDays(30);
    }


    public class MensualidadViewModel
    {
        public string Value { get; set; }  // Id como string
        public string Text { get; set; }   // Nombre de la mensualidad
        public decimal Precio { get; set; } // Precio de la mensualidad
    }

}
