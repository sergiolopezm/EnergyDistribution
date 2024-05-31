using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Server.Atributos
{
    public class ExcepcionAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(RespuestaDto.ErrorInterno())
            {
                StatusCode = 500,
            };
        }
    }
}
