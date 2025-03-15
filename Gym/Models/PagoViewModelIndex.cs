namespace Gym.Models
{
    public class PagoViewModelIndex
    {
        public int Id  { get; set; }

        public string Miembro { get; set; }
        public string Membresia { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaExpiracion { get; set; }
    }
}
