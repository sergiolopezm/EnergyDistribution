using EnergyDistribution.Shared.OutDTO;
using Microsoft.Data.SqlClient;

namespace EnergyDistribution.Domain.Services
{
    public partial class Reporte3Top20PeoresRepository
    {
        public async Task<List<Reporte3Top20PeoresOut>> ObtenerTopPeoresTramos(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Reporte3Top20PeoresOut> peoresTramos = new List<Reporte3Top20PeoresOut>();

            using (SqlConnection connection = new SqlConnection(_cadenaSQL))
            {
                await connection.OpenAsync();

                string consulta = @"
            SELECT TOP 20 *
            FROM (
                SELECT 
                    t.Nombre AS Tramo, 
                    p.Fecha, 
                    'Residencial' AS TipoUsuario, 
                    c.ResidencialWh AS ConsumoWh, 
                    cs.ResidencialCostoWh AS CostoWh, 
                    p.ResidencialPorcentaje AS PorcentajePerdida
                FROM Perdida p
                INNER JOIN Tramo t ON p.TramoId = t.Id
                INNER JOIN Consumo c ON p.TramoId = c.TramoId AND p.Fecha = c.Fecha
                INNER JOIN Costo cs ON p.TramoId = cs.TramoId AND p.Fecha = cs.Fecha
                WHERE p.Fecha BETWEEN @FechaInicio AND @FechaFin

                UNION ALL

                SELECT 
                    t.Nombre AS Tramo, 
                    p.Fecha, 
                    'Comercial' AS TipoUsuario, 
                    c.ComercialWh AS ConsumoWh, 
                    cs.ComercialCostoWh AS CostoWh, 
                    p.ComercialPorcentaje AS PorcentajePerdida
                FROM Perdida p
                INNER JOIN Tramo t ON p.TramoId = t.Id
                INNER JOIN Consumo c ON p.TramoId = c.TramoId AND p.Fecha = c.Fecha
                INNER JOIN Costo cs ON p.TramoId = cs.TramoId AND p.Fecha = cs.Fecha
                WHERE p.Fecha BETWEEN @FechaInicio AND @FechaFin

                UNION ALL

                SELECT 
                    t.Nombre AS Tramo, 
                    p.Fecha, 
                    'Industrial' AS TipoUsuario, 
                    c.IndustrialWh AS ConsumoWh, 
                    cs.IndustrialCostoWh AS CostoWh, 
                    p.IndustrialPorcentaje AS PorcentajePerdida
                FROM Perdida p
                INNER JOIN Tramo t ON p.TramoId = t.Id
                INNER JOIN Consumo c ON p.TramoId = c.TramoId AND p.Fecha = c.Fecha
                INNER JOIN Costo cs ON p.TramoId = cs.TramoId AND p.Fecha = cs.Fecha
                WHERE p.Fecha BETWEEN @FechaInicio AND @FechaFin
            ) AS Uniones
            ORDER BY PorcentajePerdida DESC";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        int index = 1;
                        while (await reader.ReadAsync())
                        {
                            peoresTramos.Add(new Reporte3Top20PeoresOut
                            {
                                Index = index++,
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

            return peoresTramos;
        }
    }
}
