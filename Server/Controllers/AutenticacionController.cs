using Microsoft.AspNetCore.Mvc;
using EnergyDistribution.Server.Atributos;
using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Server.Controllers
{

    [Log]
    [Excepcion]
    [AutorizacionJwt]
    public class AutenticacionController : Controller
    {

        [NonAction]
        public HeadersUsuarioDto HeadersUsuario()
        {
            return new HeadersUsuarioDto
            {
                IdUsuario = HttpContext.Request.Headers["IdUsuario"].FirstOrDefault()?.Split(" ").Last()!,
                Ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString()!,
            };
        }

    }
}
