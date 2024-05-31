using System.Text.Json.Serialization;
using EnergyDistribution.Util;

namespace EnergyDistribution.Shared.OutDTO
{
    public class Reporte3Top20PeoresOut
    {
        public int Index { get; set; }
        public string? Tramo { get; set; }

        [JsonConverter(typeof(CustomDateConverter))]
        public DateTime Fecha { get; set; }
        public string? TipoUsuario { get; set; }
        public double? ConsumoWh { get; set; }
        public double? CostoWh { get; set; }
        public double? PorcentajePerdida { get; set; }
    }
}
