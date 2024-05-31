using System.Globalization;

namespace EnergyDistribution.Shared.GeneralDTO
{
    public class ReportesFechasDto
    {
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public List<string> ValidacionFechas(out DateTime? fechaInicio, out DateTime? fechaFin)
        {
            List<string> errores = new List<string>();
            fechaInicio = null;
            fechaFin = null;

            if (string.IsNullOrWhiteSpace(FechaInicio))
            {
                errores.Add("La fecha de inicio es requerida y debe estar en el formato yyyy-MM-dd.");
            }
            else if (!DateTime.TryParseExact(FechaInicio, new[] { "yyyy-M-d", "yyyy-MM-d", "yyyy-M-dd", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedFechaInicio))
            {
                errores.Add("La fecha de inicio debe estar en el formato yyyy-MM-dd.");
            }
            else
            {
                fechaInicio = parsedFechaInicio;
            }

            if (string.IsNullOrWhiteSpace(FechaFin))
            {
                errores.Add("La fecha de fin es requerida y debe estar en el formato yyyy-MM-dd.");
            }
            else if (!DateTime.TryParseExact(FechaFin, new[] { "yyyy-M-d", "yyyy-MM-d", "yyyy-M-dd", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedFechaFin))
            {
                errores.Add("La fecha de fin debe estar en el formato yyyy-MM-dd.");
            }
            else
            {
                fechaFin = parsedFechaFin;
            }

            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio.Value > fechaFin.Value)
            {
                errores.Add("La fecha de inicio no puede ser mayor que la fecha de fin.");
            }

            return errores;
        }
    }
}
