using System.Diagnostics.Tracing;

namespace Gym.Models
{
    public class PagosDashPrincipal
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string Membresia { get; set; }
        public string Status { get; set; }
        public string diasRestantes { get; set; }
        public DateTime UltimoPago { get; set; }

    }


    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public int MesesInscritos { get; set; }
        public DateTime UltimoPago { get; set; }
        public DateTime ProximoPago { get; set; }
    }
}
