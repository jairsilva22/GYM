

using Gym.Models.DashPrincipal;

namespace Gym.Models
{
    public class DashPrincipalViewModel
    {
        public int Pagados { get; set; }
        public int Activos { get; set; }
        public int Vencidos { get; set; }
        public int porVencer { get; set; }

        public IEnumerable<PagosDashPrincipal> Pagos { get; set; }
        public IEnumerable<PagosVencidos> pagosVencidos { get; set; }

        public IEnumerable<PagosPorVencer> pagosPorVencer { get; set; }

    }
}
