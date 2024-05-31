using System.Text.Json.Serialization;
using EnergyDistribution.Util;

namespace EnergyDistribution.Shared.OutDTO
{
    public class Reporte1HistoricoDto
    {
        public string? Tramo { get; set; }

        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime Fecha { get; set; }
        public double? ResidencialWh { get; set; }
        public double? ComercialWh { get; set; }
        public double? IndustrialWh { get; set; }
        public double? ResidencialCostoWh { get; set; }
        public double? ComercialCostoWh { get; set; }
        public double? IndustrialCostoWh { get; set; }
        public double? ResidencialPorcentaje { get; set; }
        public double? ComercialPorcentaje { get; set; }
        public double? IndustrialPorcentaje { get; set; }
    }
}
