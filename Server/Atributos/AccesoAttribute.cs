using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Server.Atributos
{


    public class AccesoAttribute: ActionFilterAttribute
    {

        private bool _accesoValido = false;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IAccesoRepository acceso = context.HttpContext.RequestServices.GetService<IAccesoRepository>()!;
            string sitio = context.HttpContext.Request.Headers["Sitio"].FirstOrDefault()?.Split(" ").Last()!;
            string clave = context.HttpContext.Request.Headers["Clave"].FirstOrDefault()?.Split(" ").Last()!;
            
            if (sitio == null || clave == null)
            {
                _accesoValido = false;
                context.Result = new ObjectResult(RespuestaDto.ParametrosIncorrectos("Acceso invalido", "No se han enviado credenciales de acceso"))
                {
                    StatusCode = 401,
                };
            }
            else if(!acceso.ValidarAcceso(sitio!, clave!))
            {
                _accesoValido = false;
                context.Result = new ObjectResult(RespuestaDto.ParametrosIncorrectos("Acceso invalido", "Las credenciales de acceso son invalidas"))
                {
                    StatusCode = 401,
                };
            }
            else
            {
                _accesoValido = true;
            }
        }

        public bool EsValido()
        {
            return _accesoValido;
        }

    }
}
