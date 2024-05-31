using System.Globalization;

namespace EnergyDistribution.Shared.InDTO
{
    public class HojaExcelCostosIn
    {
        public string? Tramo { get; set; }
        public string? FechaTexto { get; set; }
        public DateTime? Fecha { get; set; }
        public double? ResidencialCostoWh { get; set; }
        public double? ComercialCostoWh { get; set; }
        public double? IndustrialCostoWh { get; set; }

        public List<string> ValidarParametrosHojaCostos()
        {
            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(Tramo))
            {
                errores.Add("El tramo es requerido.");
            }

            if (string.IsNullOrWhiteSpace(FechaTexto))
            {
                errores.Add("La fecha es requerida.");
            }
            else if (!DateTime.TryParseExact(FechaTexto, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                errores.Add("La fecha debe estar en el formato dd/MM/yyyy.");
            }
            else
            {
                Fecha = fecha;
            }

            if (ResidencialCostoWh == null)
            {
                errores.Add("El costo residencial es requerido y debe ser un número.");
            }

            if (ComercialCostoWh == null)
            {
                errores.Add("El costo comercial es requerido y debe ser un número.");
            }

            if (IndustrialCostoWh == null)
            {
                errores.Add("El costo industrial es requerido y debe ser un número.");
            }

            return errores;
        }
    }
}