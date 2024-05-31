﻿using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Infraestructure;
using EnergyDistribution.Shared.GeneralDTO;
using Microsoft.Extensions.Configuration;

namespace EnergyDistribution.Domain.Services
{
    public partial class Reporte1HistoricoRepository : IReporte1HistoricoRepository
    {
        private readonly DBContext _context;
        private readonly string _cadenaSQL;

        public Reporte1HistoricoRepository(IConfiguration config, DBContext context)
        {
            _context = context;
            _cadenaSQL = config.GetConnectionString("DefaultConnection") ?? "";
        }

        public RespuestaDto HistoricoConsumoTramos(ReportesFechasDto parametros)
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

            // Obtener el histórico de consumos por tramos
            try
            {
                var historicoConsumos = ObtenerHistoricoConsumosPorTramos(fechaInicio!.Value, fechaFin!.Value).Result; 

                if (historicoConsumos.Count == 0)
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
                    Detalle = "Se obtuvieron los datos históricos de consumos por tramos.",
                    Resultado = historicoConsumos
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
