using Microsoft.AspNetCore.Mvc;
using EnergyDistribution.Shared.GeneralDTO;
using EnergyDistribution.Shared.InDTO;

namespace EnergyDistribution.Server.Controllers
{
    [Route("Example/")]
    public class ExampleController : AutenticacionController
    {
        public ExampleController()
        {
        }


        [HttpPost]
        [Route("Verificar/Autenticacion")]
        public IActionResult AutenticarUsario([FromBody] UsuarioIn args)
        {
            RespuestaDto resultado = new RespuestaDto
            {
                Exito = true,
                Mensaje = "Usuario Autenticado",
                Detalle = $"El usuario {args.NombreUsuario} esta autenticado en el sistema",
                Resultado = args
            };
            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }


    }
}
