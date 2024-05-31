using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Infraestructure;
using EnergyDistribution.Shared.GeneralDTO;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace EnergyDistribution.Domain.Services
{
    public partial class Con_Cos_PerRepository : ICon_Cos_PerRepository
    {
        private readonly DBContext _context;
        private readonly string _cadenaSQL;

        public Con_Cos_PerRepository(IConfiguration config, DBContext context)
        {
            _context = context;
            _cadenaSQL = config.GetConnectionString("DefaultConnection") ?? "";
        }

        public RespuestaDto ExcelGeneral3Hojas(ExcelArgsDto args)
         {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel_div = Util.Base64isExcel.ConvertirBase64aExcel(args.archivo_b64);
            ExcelWorksheet hoja_excel_Consumo = excel_div.Workbook.Worksheets[0];
            ExcelWorksheet hoja_excel_Costos = excel_div.Workbook.Worksheets[1];
            ExcelWorksheet hoja_excel_Perdidas = excel_div.Workbook.Worksheets[2];

            List<string> erroresValidarExcel = ValidarHojas(hoja_excel_Consumo, hoja_excel_Costos, hoja_excel_Perdidas);
            if (erroresValidarExcel.Count != 0)
            {
                return new RespuestaDto
                {
                    Exito = false,
                    Mensaje = "Error",
                    Detalle = $"El archivo '{args.nombre}' no cumple con los parámetros",
                    Resultado = erroresValidarExcel
                };
            }

            List<string> erroresInsertarExcel = InsertarHojas(hoja_excel_Consumo, hoja_excel_Costos, hoja_excel_Perdidas);
            if (erroresInsertarExcel.Count != 0)
            {
                return new RespuestaDto
                {
                    Exito = false,
                    Mensaje = "Error",
                    Detalle = $"Error al insertar el archivo '{args.nombre}'",
                    Resultado = erroresInsertarExcel
                };
            }

            return new RespuestaDto
            {
                Exito = true,
                Mensaje = "Éxito",
                Detalle = $"Los datos Consumo, costos y pérdidas del archivo '{args.nombre}' fueron insertados correctamente",
                Resultado = new List<string>()
            };
           
        }
    }
}
