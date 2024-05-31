using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Server.Atributos
{

    [AttributeUsage(AttributeTargets.Method)]
    public class ValidarModeloAttribute: ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ObjectResult(ParametrosIncorrectos(context.ModelState))
                {
                    StatusCode = 400,
                };
                
            }
            
        }

        public RespuestaDto ParametrosIncorrectos(ModelStateDictionary modelState)
        {
            return new RespuestaDto()
            {
                Exito = false,
                Mensaje = "Parámetros incorrectos",
                Detalle = string.Join(". ", modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()),
                Resultado = null
            };
        }

    }


}
