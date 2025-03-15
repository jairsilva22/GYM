namespace Gym.Models
{
    public class DashboardViewModel
    {
        public decimal IngresosTotales { get; set; }
        public int CantidadPagos { get; set; }
        public int ClientesActivos { get; set; }
        public string PlanMasVendido { get; set; }
        public List<IngresoMensual> IngresosMensuales { get; set; } = new List<IngresoMensual>();
        public List<PagoViewModelIndex> Pagos { get; set; } = new List<PagoViewModelIndex>();
    }
    public class FiltroFechaModel
    {
        public int mes { get; set; }
        public int anio { get; set; }
    }
    public class IngresoMensual
    {
        public int Mes { get; set; }
        public decimal Monto { get; set; }
    }
}
