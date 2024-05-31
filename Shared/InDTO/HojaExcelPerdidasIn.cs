using System.Globalization;

namespace EnergyDistribution.Shared.InDTO
{
    public class HojaExcelPerdidasIn
    {
        public string? Tramo { get; set; }
        public string? FechaTexto { get; set; }
        public DateTime? Fecha { get; set; }
        public double? ResidencialPorcentaje { get; set; }
        public double? ComercialPorcentaje { get; set; }
        public double? IndustrialPorcentaje { get; set; }

        public List<string> ValidarHojaPerdidas()
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
            else if (!DateTime.TryParseExact(FechaTexto, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                errores.Add("La fecha debe estar en el formato yyyy-MM-dd.");
            }
            else
            {
                Fecha = fecha;
            }

            decimal number;
            if (string.IsNullOrEmpty(ResidencialPorcentaje.ToString()) || !Decimal.TryParse(ResidencialPorcentaje.ToString(), out number))
            {
                errores.Add("El porcentaje residencial es requerido.");
            }

            if (string.IsNullOrEmpty(ComercialPorcentaje.ToString()) || !Decimal.TryParse(ComercialPorcentaje.ToString(), out number))
            {
                errores.Add("El porcentaje comercial es requerido.");
            }

            if (string.IsNullOrEmpty(IndustrialPorcentaje.ToString()) || !Decimal.TryParse(IndustrialPorcentaje.ToString(), out number))
            {
                errores.Add("El porcentaje industrial es requerido.");
            }

            return errores;
        }

    }
}
