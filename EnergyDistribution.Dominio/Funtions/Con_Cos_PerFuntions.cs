using EnergyDistribution.Infraestructure;
using EnergyDistribution.Shared.InDTO;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace EnergyDistribution.Domain.Services
{
    public partial class Con_Cos_PerRepository
    {
        List<HojaExcelConsumoIn> consumos = new List<HojaExcelConsumoIn>();
        List<HojaExcelCostosIn> costos = new List<HojaExcelCostosIn>();
        List<HojaExcelPerdidasIn> perdidas = new List<HojaExcelPerdidasIn>();

        private bool DatosExisten<T>(SqlConnection connection, SqlTransaction transaction, string nombreTabla, T dato)
        {
            var propiedades = typeof(T).GetProperties();
            string consulta = $"SELECT COUNT(1) FROM {nombreTabla} WHERE ";

            List<string> condiciones = new List<string>();
            foreach (var propiedad in propiedades)
            {
                if (propiedad.Name != "Id" && propiedad.Name != "Tramo" && propiedad.GetValue(dato) != null)
                {
                    condiciones.Add($"{propiedad.Name} = @{propiedad.Name}");
                }
            }

            consulta += string.Join(" AND ", condiciones);

            using (SqlCommand command = new SqlCommand(consulta, connection, transaction))
            {
                foreach (var propiedad in propiedades)
                {
                    if (propiedad.Name != "Id" && propiedad.Name != "Tramo" && propiedad.GetValue(dato) != null)
                    {
                        command.Parameters.AddWithValue($"@{propiedad.Name}", propiedad.GetValue(dato));
                    }
                }

                return (int)command.ExecuteScalar() > 0;
            }
        }

        #region Variables Globales y Validaciones de Hojas
        public List<string> ValidarHojas(ExcelWorksheet hoja_excel_Consumo, ExcelWorksheet hoja_excel_Costos, ExcelWorksheet hoja_excel_Perdidas)
        {
            List<string> errores = new List<string>();
            errores.AddRange(ValidarHojaConsumo(hoja_excel_Consumo));
            errores.AddRange(ValidarHojaCostos(hoja_excel_Costos));
            errores.AddRange(ValidarHojaPerdidas(hoja_excel_Perdidas));
            return errores;
        }

        private List<string> ValidarHojaConsumo(ExcelWorksheet hoja_excel_Consumo)
        {
            List<string> errores = new List<string>();
            int fil = hoja_excel_Consumo.Dimension.End.Row;

            for (int i = 2; i <= fil; i++)
            {
                HojaExcelConsumoIn hojaExcelConsumoIn = new HojaExcelConsumoIn();
                try
                {
                    hojaExcelConsumoIn.Tramo = hoja_excel_Consumo.Cells[i, 1].Value?.ToString();
                    hojaExcelConsumoIn.FechaTexto = GetCellDateAsString(hoja_excel_Consumo.Cells[i, 2], errores, i, "yyyy-MM-dd");
                    hojaExcelConsumoIn.ResidencialWh = string.IsNullOrEmpty(hoja_excel_Consumo.Cells[i, 3].Text) ? (double?)null : double.Parse(hoja_excel_Consumo.Cells[i, 3].Text);
                    hojaExcelConsumoIn.ComercialWh = string.IsNullOrEmpty(hoja_excel_Consumo.Cells[i, 4].Text) ? (double?)null : double.Parse(hoja_excel_Consumo.Cells[i, 4].Text);
                    hojaExcelConsumoIn.IndustrialWh = string.IsNullOrEmpty(hoja_excel_Consumo.Cells[i, 5].Text) ? (double?)null : double.Parse(hoja_excel_Consumo.Cells[i, 5].Text);

                    var validacionErrores = hojaExcelConsumoIn.ExcelHojaConsumo();
                    if (validacionErrores.Count > 0)
                    {
                        foreach (var error in validacionErrores)
                        {
                            errores.Add($"Hoja Consumo - Fila {i}: {error}");
                        }
                    }
                }
                catch
                {
                    errores.Add($"Hoja Consumo - Fila {i} contiene celdas sin información o con información corrupta");
                }
            }

            return errores;
        }

        private List<string> ValidarHojaCostos(ExcelWorksheet hoja_excel_Costos)
        {
            List<string> errores = new List<string>();
            int fil = hoja_excel_Costos.Dimension.End.Row;

            for (int i = 2; i <= fil; i++)
            {
                HojaExcelCostosIn hojaExcelCostosIn = new HojaExcelCostosIn();
                try
                {
                    hojaExcelCostosIn.Tramo = hoja_excel_Costos.Cells[i, 1].Value?.ToString()?.Trim();
                    hojaExcelCostosIn.FechaTexto = GetCellDateAsString(hoja_excel_Costos.Cells[i, 2], errores, i, "dd/MM/yyyy");
                    hojaExcelCostosIn.ResidencialCostoWh = string.IsNullOrEmpty(hoja_excel_Costos.Cells[i, 3].Text) ? (double?)null : double.Parse(hoja_excel_Costos.Cells[i, 3].Text.Trim());
                    hojaExcelCostosIn.ComercialCostoWh = string.IsNullOrEmpty(hoja_excel_Costos.Cells[i, 4].Text) ? (double?)null : double.Parse(hoja_excel_Costos.Cells[i, 4].Text.Trim());
                    hojaExcelCostosIn.IndustrialCostoWh = string.IsNullOrEmpty(hoja_excel_Costos.Cells[i, 5].Text) ? (double?)null : double.Parse(hoja_excel_Costos.Cells[i, 5].Text.Trim());

                    var validacionErrores = hojaExcelCostosIn.ValidarParametrosHojaCostos();
                    if (validacionErrores.Count > 0)
                    {
                        foreach (var error in validacionErrores)
                        {
                            errores.Add($"Hoja Costos - Fila {i}: {error}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    errores.Add($"Hoja Costos - Fila {i} contiene celdas sin información o con información corrupta: {ex.Message}");
                }
            }

            return errores;
        }

        private List<string> ValidarHojaPerdidas(ExcelWorksheet hoja_excel_Perdidas)
        {
            List<string> errores = new List<string>();
            int fil = hoja_excel_Perdidas.Dimension.End.Row;

            for (int i = 2; i <= fil; i++)
            {
                HojaExcelPerdidasIn hojaExcelPerdidasIn = new HojaExcelPerdidasIn();
                try
                {
                    hojaExcelPerdidasIn.Tramo = hoja_excel_Perdidas.Cells[i, 1].Value?.ToString()?.Trim();
                    hojaExcelPerdidasIn.FechaTexto = GetCellDateAsString(hoja_excel_Perdidas.Cells[i, 2], errores, i, "yyyy-MM-dd");

                    hojaExcelPerdidasIn.ResidencialPorcentaje = TryParseDouble(hoja_excel_Perdidas.Cells[i, 3].Text, errores, i, "Residencial");
                    hojaExcelPerdidasIn.ComercialPorcentaje = TryParseDouble(hoja_excel_Perdidas.Cells[i, 4].Text, errores, i, "Comercial");
                    hojaExcelPerdidasIn.IndustrialPorcentaje = TryParseDouble(hoja_excel_Perdidas.Cells[i, 5].Text, errores, i, "Industrial");

                    var validacionErrores = hojaExcelPerdidasIn.ValidarHojaPerdidas();
                    if (validacionErrores.Count > 0)
                    {
                        foreach (var error in validacionErrores)
                        {
                            errores.Add($"Hoja Pérdidas - Fila {i}: {error}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    errores.Add($"Hoja Pérdidas - Fila {i} contiene celdas sin información o con información corrupta: {ex.Message}");
                }
            }

            return errores;
        }



        #endregion Variables Globales y Validaciones de Hojas

        #region Datos de Hojas Consumo, Costos y Pérdidas
        public List<string> InsertarHojas(ExcelWorksheet hoja_excel_Consumo, ExcelWorksheet hoja_excel_Costos, ExcelWorksheet hoja_excel_Perdidas)
        {
            List<string> errores = new List<string>();

            using (SqlConnection connection = new SqlConnection(_cadenaSQL))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    Dictionary<string, int> tramos = CargarTramos(connection, transaction);

                    List<Consumo> consumosParaInsertar = ObtenerConsumos(hoja_excel_Consumo, errores, tramos, connection, transaction);
                    List<Costo> costosParaInsertar = ObtenerCostos(hoja_excel_Costos, errores, tramos, connection, transaction);
                    List<Perdidum> perdidasParaInsertar = ObtenerPerdidas(hoja_excel_Perdidas, errores, tramos, connection, transaction);

                    if (errores.Count == 0)
                    {
                        InsertarDatosEnTabla(connection, transaction, "Consumo", consumosParaInsertar, errores);
                        InsertarDatosEnTabla(connection, transaction, "Costo", costosParaInsertar, errores);
                        InsertarDatosEnTabla(connection, transaction, "Perdida", perdidasParaInsertar, errores);

                        if (errores.Count == 0)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    errores.Add($"Error en la transacción: {ex.Message}");
                }
            }

            return errores;
        }

        private List<Consumo> ObtenerConsumos(ExcelWorksheet hoja_excel_Consumo, List<string> errores, Dictionary<string, int> tramos, SqlConnection connection, SqlTransaction transaction)
        {
            List<Consumo> consumosParaInsertar = new List<Consumo>();
            int fil = hoja_excel_Consumo.Dimension.End.Row;

            for (int i = 2; i <= fil; i++)
            {
                try
                {
                    HojaExcelConsumoIn hojaExcelConsumoIn = new HojaExcelConsumoIn
                    {
                        Tramo = hoja_excel_Consumo.Cells[i, 1].Value?.ToString(),
                        FechaTexto = GetCellDateAsString(hoja_excel_Consumo.Cells[i, 2], errores, i, "yyyy-MM-dd"),
                        ResidencialWh = string.IsNullOrEmpty(hoja_excel_Consumo.Cells[i, 3].Text) ? (double?)null : double.Parse(hoja_excel_Consumo.Cells[i, 3].Text),
                        ComercialWh = string.IsNullOrEmpty(hoja_excel_Consumo.Cells[i, 4].Text) ? (double?)null : double.Parse(hoja_excel_Consumo.Cells[i, 4].Text),
                        IndustrialWh = string.IsNullOrEmpty(hoja_excel_Consumo.Cells[i, 5].Text) ? (double?)null : double.Parse(hoja_excel_Consumo.Cells[i, 5].Text)
                    };

                    var validacionErrores = hojaExcelConsumoIn.ExcelHojaConsumo();
                    if (validacionErrores.Count > 0)
                    {
                        foreach (var error in validacionErrores)
                        {
                            errores.Add($"Fila {i}: {error}");
                        }
                        continue;
                    }

                    if (!tramos.TryGetValue(hojaExcelConsumoIn.Tramo, out int tramoId))
                    {
                        errores.Add($"Fila {i}: El tramo '{hojaExcelConsumoIn.Tramo}' no existe en la base de datos.");
                        continue;
                    }

                    Consumo consumo = new Consumo
                    {
                        TramoId = tramoId,
                        Fecha = hojaExcelConsumoIn.Fecha,
                        ResidencialWh = hojaExcelConsumoIn.ResidencialWh,
                        ComercialWh = hojaExcelConsumoIn.ComercialWh,
                        IndustrialWh = hojaExcelConsumoIn.IndustrialWh
                    };

                    if (!DatosExisten(connection, transaction, "Consumo", consumo))
                    {
                        consumosParaInsertar.Add(consumo);
                    }
                    else
                    {
                        errores.Add($"Fila {i}: Los datos ya existen en la tabla Consumo.");
                    }
                }
                catch (Exception ex)
                {
                    errores.Add($"Fila {i}: {ex.Message}");
                }
            }

            return consumosParaInsertar;
        }

        private List<Costo> ObtenerCostos(ExcelWorksheet hoja_excel_Costos, List<string> errores, Dictionary<string, int> tramos, SqlConnection connection, SqlTransaction transaction)
        {
            List<Costo> costosParaInsertar = new List<Costo>();
            int fil = hoja_excel_Costos.Dimension.End.Row;

            for (int i = 2; i <= fil; i++)
            {
                try
                {
                    HojaExcelCostosIn hojaExcelCostosIn = new HojaExcelCostosIn
                    {
                        Tramo = hoja_excel_Costos.Cells[i, 1].Value?.ToString()?.Trim(),
                        FechaTexto = GetCellDateAsString(hoja_excel_Costos.Cells[i, 2], errores, i, "dd/MM/yyyy"),
                        ResidencialCostoWh = string.IsNullOrEmpty(hoja_excel_Costos.Cells[i, 3].Text) ? (double?)null : double.Parse(hoja_excel_Costos.Cells[i, 3].Text.Trim()),
                        ComercialCostoWh = string.IsNullOrEmpty(hoja_excel_Costos.Cells[i, 4].Text) ? (double?)null : double.Parse(hoja_excel_Costos.Cells[i, 4].Text.Trim()),
                        IndustrialCostoWh = string.IsNullOrEmpty(hoja_excel_Costos.Cells[i, 5].Text) ? (double?)null : double.Parse(hoja_excel_Costos.Cells[i, 5].Text.Trim())
                    };

                    var validacionErrores = hojaExcelCostosIn.ValidarParametrosHojaCostos();
                    if (validacionErrores.Count > 0)
                    {
                        foreach (var error in validacionErrores)
                        {
                            errores.Add($"Fila {i}: {error}");
                        }
                        continue;
                    }

                    if (!tramos.TryGetValue(hojaExcelCostosIn.Tramo, out int tramoId))
                    {
                        errores.Add($"Fila {i}: El tramo '{hojaExcelCostosIn.Tramo}' no existe en la base de datos.");
                        continue;
                    }

                    Costo costo = new Costo
                    {
                        TramoId = tramoId,
                        Fecha = hojaExcelCostosIn.Fecha,
                        ResidencialCostoWh = hojaExcelCostosIn.ResidencialCostoWh,
                        ComercialCostoWh = hojaExcelCostosIn.ComercialCostoWh,
                        IndustrialCostoWh = hojaExcelCostosIn.IndustrialCostoWh
                    };

                    if (!DatosExisten(connection, transaction, "Costo", costo))
                    {
                        costosParaInsertar.Add(costo);
                    }
                    else
                    {
                        errores.Add($"Fila {i}: Los datos ya existen en la tabla Costo.");
                    }
                }
                catch (Exception ex)
                {
                    errores.Add($"Fila {i}: {ex.Message}");
                }
            }

            return costosParaInsertar;
        }

        private List<Perdidum> ObtenerPerdidas(ExcelWorksheet hoja_excel_Perdidas, List<string> errores, Dictionary<string, int> tramos, SqlConnection connection, SqlTransaction transaction)
        {
            List<Perdidum> perdidasParaInsertar = new List<Perdidum>();
            int fil = hoja_excel_Perdidas.Dimension.End.Row;

            for (int i = 2; i <= fil; i++)
            {
                try
                {
                    HojaExcelPerdidasIn hojaExcelPerdidasIn = new HojaExcelPerdidasIn
                    {
                        Tramo = hoja_excel_Perdidas.Cells[i, 1].Value?.ToString()?.Trim(),
                        FechaTexto = GetCellDateAsString(hoja_excel_Perdidas.Cells[i, 2], errores, i, "yyyy-MM-dd"),
                        ResidencialPorcentaje = TryParseDouble(hoja_excel_Perdidas.Cells[i, 3].Text, errores, i, "Residencial"),
                        ComercialPorcentaje = TryParseDouble(hoja_excel_Perdidas.Cells[i, 4].Text, errores, i, "Comercial"),
                        IndustrialPorcentaje = TryParseDouble(hoja_excel_Perdidas.Cells[i, 5].Text, errores, i, "Industrial")
                    };

                    var validacionErrores = hojaExcelPerdidasIn.ValidarHojaPerdidas();
                    if (validacionErrores.Count > 0)
                    {
                        foreach (var error in validacionErrores)
                        {
                            errores.Add($"Fila {i}: {error}");
                        }
                        continue;
                    }

                    if (!tramos.TryGetValue(hojaExcelPerdidasIn.Tramo, out int tramoId))
                    {
                        errores.Add($"Fila {i}: El tramo '{hojaExcelPerdidasIn.Tramo}' no existe en la base de datos.");
                        continue;
                    }

                    Perdidum perdida = new Perdidum
                    {
                        TramoId = tramoId,
                        Fecha = hojaExcelPerdidasIn.Fecha,
                        ResidencialPorcentaje = hojaExcelPerdidasIn.ResidencialPorcentaje,
                        ComercialPorcentaje = hojaExcelPerdidasIn.ComercialPorcentaje,
                        IndustrialPorcentaje = hojaExcelPerdidasIn.IndustrialPorcentaje,
                    };

                    if (!DatosExisten(connection, transaction, "Perdida", perdida))
                    {
                        perdidasParaInsertar.Add(perdida);
                    }
                    else
                    {
                        errores.Add($"Fila {i}: Los datos ya existen en la tabla Perdida.");
                    }
                }
                catch (Exception ex)
                {
                    errores.Add($"Fila {i}: {ex.Message}");
                }
            }

            return perdidasParaInsertar;
        }

        #endregion Datos de Hojas Consumo, Costos y Pérdidas


        private void InsertarDatosEnTabla<T>(SqlConnection connection, SqlTransaction transaction, string nombreTabla, List<T> datos, List<string> errores)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                bulkCopy.BulkCopyTimeout = 3600;
                bulkCopy.DestinationTableName = nombreTabla;

                DataTable dataTable = new DataTable();

                // Obtener las propiedades de la clase T
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

                // Crear las columnas del DataTable basadas en las propiedades de T
                foreach (PropertyDescriptor prop in properties)
                {
                    if (prop.Name != "Id" && prop.Name != "Tramo")
                    {
                        dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    }
                }

                foreach (T item in datos)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (PropertyDescriptor prop in properties)
                    {
                        if (prop.Name != "Id" && prop.Name != "Tramo")
                        {
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        }
                    }
                    dataTable.Rows.Add(row);
                }

                // Información de depuración: nombres de columnas en el DataTable
                Console.WriteLine($"Columnas en el DataTable: {string.Join(", ", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName))}");

                // Información de depuración: nombres de columnas en la tabla destino
                List<string> columnasDestino = new List<string>();
                using (SqlCommand command = new SqlCommand($"SELECT TOP 0 * FROM {nombreTabla}", connection, transaction))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable schemaTable = reader.GetSchemaTable();
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            columnasDestino.Add(row["ColumnName"].ToString()!);
                        }
                    }
                }

                Console.WriteLine($"Columnas en la tabla {nombreTabla}: {string.Join(", ", columnasDestino)}");

                // Mapeo de columnas excluyendo propiedades no necesarias
                foreach (var property in typeof(T).GetProperties())
                {
                    if (property.Name != "Id" && property.Name != "Tramo" && columnasDestino.Contains(property.Name))
                    {
                        bulkCopy.ColumnMappings.Add(property.Name, property.Name);
                    }
                    else if (!columnasDestino.Contains(property.Name) && property.Name != "Tramo" && property.Name != "Id")
                    {
                        errores.Add($"La columna {property.Name} no existe en la tabla {nombreTabla}.");
                    }
                }

                try
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        bulkCopy.WriteToServer(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    errores.Add($"Error en la inserción masiva en la tabla {nombreTabla}: {ex.Message}");
                }
            }
        }

        private string GetCellDateAsString(ExcelRange cell, List<string> errores, int fila, string formatoEsperado)
        {
            if (cell.Value == null)
            {
                errores.Add($"Fila {fila}: La fecha es requerida.");
                return string.Empty;
            }

            DateTime fecha;
            string fechaTexto = (cell.Text ?? cell.Value.ToString()!).Trim();

            Console.WriteLine($"Fila {fila}: Valor de la celda (Text): {cell.Text}");
            Console.WriteLine($"Fila {fila}: Valor de la celda (Value): {cell.Value.ToString()}");
            Console.WriteLine($"Fila {fila}: Fecha Texto: {fechaTexto}");

            string[] formatos = { "yyyy-MM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "d/M/yyyy", "d/MM/yyyy" }; // Agrega más formatos si es necesario
            bool parseExitoso = DateTime.TryParseExact(fechaTexto, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            if (parseExitoso)
            {
                return fecha.ToString(formatoEsperado);
            }

            if (double.TryParse(fechaTexto, out double oaDate))
            {
                fecha = DateTime.FromOADate(oaDate);
                return fecha.ToString(formatoEsperado);
            }

            errores.Add($"Fila {fila}: La fecha debe estar en el formato {formatoEsperado}.");
            return string.Empty;
        }

        private int? GetTramoIdByName(string? tramoName, List<string> errores, int fila)
        {
            if (string.IsNullOrEmpty(tramoName))
            {
                errores.Add($"Fila {fila}: Nombre del tramo es nulo o vacío.");
                return null;
            }

            var tramo = _context.Tramos.FirstOrDefault(t => t.Nombre == tramoName);
            if (tramo == null)
            {
                errores.Add($"Fila {fila}: No se encontró un tramo con el nombre {tramoName}");
                return null;
            }
            return tramo.Id;
        }

        private double? TryParseDouble(string text, List<string> errores, int fila, string campo)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            else
            {
                errores.Add($"Hoja Pérdidas - Fila {fila}: El valor del campo {campo} no es un número válido.");
                return null;
            }
        }

        private Dictionary<string, int> CargarTramos(SqlConnection connection, SqlTransaction transaction)
        {
            Dictionary<string, int> tramos = new Dictionary<string, int>();

            using (SqlCommand command = new SqlCommand("SELECT Id, Nombre FROM Tramo", connection, transaction))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tramos[reader["Nombre"].ToString()!] = (int)reader["Id"];
                    }
                }
            }

            return tramos;
        }
    }
}