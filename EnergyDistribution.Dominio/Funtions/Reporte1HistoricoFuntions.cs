using EnergyDistribution.Shared.OutDTO;
using Microsoft.Data.SqlClient;

namespace EnergyDistribution.Domain.Services
{
    public partial class Reporte1HistoricoRepository
    {
        public async Task<List<Reporte1HistoricoDto>> ObtenerHistoricoConsumosPorTramos(DateTime fechaInicio, DateTime fechaFin)
        {
            List<Reporte1HistoricoDto> historicoConsumos = new List<Reporte1HistoricoDto>();

            using (SqlConnection connection = new SqlConnection(_cadenaSQL))
            {
                connection.Open();

                string consulta = @"
                    SELECT 
                        t.Nombre AS Tramo,
                        c.Fecha,
                        c.ResidencialWh,
                        c.ComercialWh,
                        c.IndustrialWh,
                        cs.ResidencialCostoWh,
                        cs.ComercialCostoWh,
                        cs.IndustrialCostoWh,
                        p.ResidencialPorcentaje,
                        p.ComercialPorcentaje,
                        p.IndustrialPorcentaje
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
                    ORDER BY 
                        t.Nombre, c.Fecha";

                using (SqlCommand command = new SqlCommand(consulta, connection))
                {
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            historicoConsumos.Add(new Reporte1HistoricoDto
                            {
                                Tramo = reader["Tramo"].ToString(),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                ResidencialWh = reader["ResidencialWh"] != DBNull.Value ? Convert.ToDouble(reader["ResidencialWh"]) : (double?)null,
                                ComercialWh = reader["ComercialWh"] != DBNull.Value ? Convert.ToDouble(reader["ComercialWh"]) : (double?)null,
                                IndustrialWh = reader["IndustrialWh"] != DBNull.Value ? Convert.ToDouble(reader["IndustrialWh"]) : (double?)null,
                                ResidencialCostoWh = reader["ResidencialCostoWh"] != DBNull.Value ? Convert.ToDouble(reader["ResidencialCostoWh"]) : (double?)null,
                                ComercialCostoWh = reader["ComercialCostoWh"] != DBNull.Value ? Convert.ToDouble(reader["ComercialCostoWh"]) : (double?)null,
                                IndustrialCostoWh = reader["IndustrialCostoWh"] != DBNull.Value ? Convert.ToDouble(reader["IndustrialCostoWh"]) : (double?)null,
                                ResidencialPorcentaje = reader["ResidencialPorcentaje"] != DBNull.Value ? Convert.ToDouble(reader["ResidencialPorcentaje"]) : (double?)null,
                                ComercialPorcentaje = reader["ComercialPorcentaje"] != DBNull.Value ? Convert.ToDouble(reader["ComercialPorcentaje"]) : (double?)null,
                                IndustrialPorcentaje = reader["IndustrialPorcentaje"] != DBNull.Value ? Convert.ToDouble(reader["IndustrialPorcentaje"]) : (double?)null,
                            });
                        }
                    }
                }
            }

            return historicoConsumos;
        }

    }
}
