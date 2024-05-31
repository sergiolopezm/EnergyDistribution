using System;
using System.Collections.Generic;

namespace EnergyDistribution.Infraestructure
{
    public partial class Consumo
    {
        public int Id { get; set; }
        public int? TramoId { get; set; }
        public DateTime? Fecha { get; set; }
        public double? ResidencialWh { get; set; }
        public double? ComercialWh { get; set; }
        public double? IndustrialWh { get; set; }

        public virtual Tramo? Tramo { get; set; }
    }
}
