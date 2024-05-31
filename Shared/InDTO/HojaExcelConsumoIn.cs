using System.Globalization;

namespace EnergyDistribution.Shared.InDTO
{
    public class HojaExcelConsumoIn
    {
        public string? Tramo { get; set; }
        public string? FechaTexto { get; set; }
        public DateTime? Fecha { get; set; }
        public double? ResidencialWh { get; set; }
        public double? ComercialWh { get; set; }
        public double? IndustrialWh { get; set; }

        public List<string> ExcelHojaConsumo()
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

            if (ResidencialWh == null)
            {
                errores.Add("El consumo residencial es requerido.");
            }
            else if (!IsWholeNumber(ResidencialWh.ToString()!))
            {
                errores.Add("El consumo residencial debe ser un número entero válido y no contener decimales.");
            }

            if (ComercialWh == null)
            {
                errores.Add("El consumo comercial es requerido.");
            }
            else if (!IsWholeNumber(ComercialWh.ToString()!))
            {
                errores.Add("El consumo comercial debe ser un número entero válido y no contener decimales.");
            }

            if (IndustrialWh == null)
            {
                errores.Add("El consumo industrial es requerido.");
            }
            else if (!IsWholeNumber(IndustrialWh.ToString()!))
            {
                errores.Add("El consumo industrial debe ser un número entero válido y no contener decimales.");
            }

            return errores;
        }

        private bool IsWholeNumber(string value)
        {
            return int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out _);
        }
    }
}