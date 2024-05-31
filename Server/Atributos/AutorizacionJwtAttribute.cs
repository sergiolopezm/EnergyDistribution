using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Org.BouncyCastle.Asn1.Ocsp;
using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Server.Atributos
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AutorizacionJwtAttribute: ActionFilterAttribute
    {

        readonly AccesoAttribute acceso = new AccesoAttribute();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            acceso.OnActionExecuting(context);
            if(acceso.EsValido())
            {
                ITokenRepository token = context.HttpContext.RequestServices.GetService<ITokenRepository>()!;
                
                string idToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
                string idUsuario = context.HttpContext.Request.Headers["IdUsuario"].FirstOrDefault()?.Split(" ").Last()!;
                string ip = context.HttpContext.Connection.RemoteIpAddress?.ToString()!;

                ValidoDTO valido = token.EsValido(idToken!, idUsuario, ip);
                if( !valido.EsValido )
                {
                    context.Result = new ObjectResult(RespuestaDto.ParametrosIncorrectos("Sesión invalida", valido.Detalle!))
                    {
                        StatusCode = 401,
                    };
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ITokenRepository token = context.HttpContext.RequestServices.GetService<ITokenRepository>()!;
            string idToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()!;
            token.AumentarTiempoExpiracion(idToken);
        }


    }
}
