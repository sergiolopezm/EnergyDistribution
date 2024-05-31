using System;
using System.Collections.Generic;

namespace EnergyDistribution.Infraestructure
{
    public partial class Costo
    {
        public int Id { get; set; }
        public int? TramoId { get; set; }
        public DateTime? Fecha { get; set; }
        public double? ResidencialCostoWh { get; set; }
        public double? ComercialCostoWh { get; set; }
        public double? IndustrialCostoWh { get; set; }

        public virtual Tramo? Tramo { get; set; }
    }
}
