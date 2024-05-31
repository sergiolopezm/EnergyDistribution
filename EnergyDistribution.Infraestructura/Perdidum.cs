namespace EnergyDistribution.Infraestructure
{
    public partial class Perdidum
    {
        public int Id { get; set; }
        public int? TramoId { get; set; }
        public DateTime? Fecha { get; set; }
        public double? ResidencialPorcentaje { get; set; }
        public double? ComercialPorcentaje { get; set; }
        public double? IndustrialPorcentaje { get; set; }

        public virtual Tramo? Tramo { get; set; }
    }
}
