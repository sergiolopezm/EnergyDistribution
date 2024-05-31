using EnergyDistribution.Shared.ValidModel;
using System.ComponentModel.DataAnnotations;

namespace EnergyDistribution.Shared.InDTO
{
    public class UsuarioIn
    {

        [Required(ErrorMessage = "El usuario es requerido")]
        public string? NombreUsuario { get; set; }

        //[EsIP]
        //[Required(ErrorMessage = "La ip es requerida")]
        public string? Ip { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Contraseña { get; set; }

    }

}
