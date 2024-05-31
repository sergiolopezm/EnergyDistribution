using EnergyDistribution.Shared.OutDTO;
using Microsoft.Data.SqlClient;

namespace EnergyDistribution.Domain.Services
{
    public partial class Reporte2HistoricoRepository
    {
        public async Task<List<Reporte2HistoricoDto>> ObtenerHistoricoConsumosPorCliente(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Reporte2HistoricoDto> historicoConsumos = new List<Reporte2HistoricoDto>();

            using (SqlConnection connection = new SqlConnection(_cadenaSQL))
            {
                await connection.OpenAsync();

                string consulta = @"
                    SELECT 
                        t.Nombre AS Tramo,
                        c.Fecha,
                        'Residencial' AS TipoUsuario,
                        c.ResidencialWh AS ConsumoWh,
                        cs.ResidencialCostoWh AS CostoWh,
                        p.ResidencialPorcentaje AS PorcentajePerdida
                    FROM 
                        Consumo c
                    INNER JOIN 
                        Tramo t ON c.TramoId = t.Id
                    INNER JOIN 
                        Costo cs ON c.TramoId = cs.TramoId AND c.Fecha = cs.Fecha
                    INNER JOIN 
                        Perdida p ON c.TramoId = p.TramoId AND c.Fecha = p.Fecha
                    WHERE 
                        c.Fecha BETWEEN @FechaInicio AND @FechaFin
                    UNION
                    SELECT 
                        t.Nombre AS Tramo,
                        c.Fecha,
                        'Comercial' AS TipoUsuario,
                        c.ComercialWh AS ConsumoWh,
                        cs.ComercialCostoWh AS CostoWh,
                        p.ComercialPorcentaje AS PorcentajePerdida
                    FROM 
                        Consumo c
                    INNER JOIN 
                        Tramo t ON c.TramoId = t.Id
                    INNER JOIN 
                        Costo cs ON c.TramoId = cs.TramoId AND c.Fecha = cs.Fecha
                    INNER JOIN 
                        Perdida p ON c.TramoId = p.TramoId AND c.Fecha = p.Fecha
                    WHERE 
                        c.Fecha BETWEEN @FechaInicio AND @FechaFin
                    UNION
                    SELECT 
                        t.Nombre AS Tramo,
                        c.Fecha,
                        'Industrial' AS TipoUsuario,
                        c.IndustrialWh AS ConsumoWh,
                        cs.IndustrialCostoWh AS CostoWh,
                        p.IndustrialPorcentaje AS PorcentajePerdida
                    FROM 
                        Consumo c
                    INNER JOIN 
                        Tramo t ON c.TramoId = t.Id
                    INNER JOIN 
                        Costo cs ON c.TramoId = cs.TramoId AND c.Fecha = cs.Fecha
                    INNER JOIN 
                        Perdida p ON c.TramoId = p.TramoId AND c.Fecha = p.Fecha
                    WHERE 
                        c.Fecha BETWEEN @FechaInicio AND @FechaFin
                    ORDER BY c.Fecha, t.Nombre";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            historicoConsumos.Add(new Reporte2HistoricoDto
                            {
                                Tramo = reader["Tramo"].ToString(),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                TipoUsuario = reader["TipoUsuario"].ToString(),
                                ConsumoWh = reader["ConsumoWh"] != DBNull.Value ? Convert.ToDouble(reader["ConsumoWh"]) : (double?)null,
                                CostoWh = reader["CostoWh"] != DBNull.Value ? Convert.ToDouble(reader["CostoWh"]) : (double?)null,
                                PorcentajePerdida = reader["PorcentajePerdida"] != DBNull.Value ? Convert.ToDouble(reader["PorcentajePerdida"]) : (double?)null,
                            });
                        }
                    }
                }
            }

            return historicoConsumos;
        }
    }
}
