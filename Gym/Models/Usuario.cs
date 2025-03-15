namespace Gym.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }

        public string Telefono { get; set; }
        
        public DateTime FechaAlta { get; set; } = DateTime.Today;

      

    }

}
