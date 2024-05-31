using EnergyDistribution.Util;
using System.ComponentModel.DataAnnotations;

namespace EnergyDistribution.Shared.GeneralDTO
{
    public class ExcelArgsDto
    {
        [Required(ErrorMessage = "El nombre del archivo es requerido.")]
        [StringLength(50, ErrorMessage = "El nombre del archivo debe tener como máximo 50 caracteres.")]
        public string? nombre { get; set; }

        [Required(ErrorMessage = "El archivo es requerido.")]
        [Base64isExcel(ErrorMessage = "El archivo agregado no corresponde a un Excel")]
        public string? archivo_b64 { get; set; }

    }
}
