using Microsoft.AspNetCore.Mvc;
using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Server.Atributos;
using EnergyDistribution.Shared.GeneralDTO;
using EnergyDistribution.Shared.InDTO;

namespace EnergyDistribution.Server.Controllers
{

    [Excepcion]
    [Log]
    [Route("api/Usuario/")]
    public class UsuarioController : Controller
    {

        private readonly IUsuarioRepository _repository;

        public UsuarioController(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        
        [HttpPost]
        [Acceso]
        [ValidarModelo]
        [Route("Autenticar")]
        public IActionResult AutenticarUsario([FromBody] UsuarioIn args) 
        {
            RespuestaDto resultado = _repository.AutenticarUsuario(args, Ip());
            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }

        [NonAction]
        private string Ip()
        {
            return Request.HttpContext.Connection.RemoteIpAddress?.ToString()!;
        }

    }
}
