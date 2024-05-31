using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Infraestructure;
using EnergyDistribution.Shared.GeneralDTO;
using Microsoft.Extensions.Configuration;

namespace EnergyDistribution.Domain.Services
{
    public partial class Reporte3Top20PeoresRepository : IReporte3Top20PeoresRepository
    {
        private readonly DBContext _context;
        private readonly string _cadenaSQL;

        public Reporte3Top20PeoresRepository(IConfiguration config, DBContext context)
        {
            _context = context;
            _cadenaSQL = config.GetConnectionString("DefaultConnection") ?? "";
        }

        public RespuestaDto ObtenerTopPeoresTramos(ReportesFechasDto parametros)
        {
            var errores = parametros.ValidacionFechas(out DateTime? fechaInicio, out DateTime? fechaFin);
            if (errores.Count > 0)
            {
                return new RespuestaDto
                {
                    Exito = false,
                    Mensaje = "Error en los parámetros",
                    Detalle = "Los parámetros proporcionados son inválidos.",
                    Resultado = errores
                };
            }

            try
            {
                var peoresTramos = ObtenerTopPeoresTramos(fechaInicio!.Value, fechaFin!.Value).Result;

                if (peoresTramos.Count == 0)
                {
                    return new RespuestaDto
                    {
                        Exito = true,
                        Mensaje = "No se encontraron datos",
                        Detalle = "No se encontraron registros para las fechas proporcionadas.",
                        Resultado = new List<string>()
                    };
                }

                return new RespuestaDto
                {
                    Exito = true,
                    Mensaje = "Datos obtenidos correctamente",
                    Detalle = "Se obtuvieron los datos de los 20 peores tramos/clientes con mayores pérdidas.",
                    Resultado = peoresTramos
                };
            }
            catch (Exception ex)
            {
                return new RespuestaDto
                {
                    Exito = false,
                    Mensaje = "Error",
                    Detalle = $"Ocurrió un error al obtener los datos: {ex.Message}",
                    Resultado = new List<string>()
                };
            }
        }
    }
}
