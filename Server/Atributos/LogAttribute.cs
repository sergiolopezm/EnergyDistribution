using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Server.Atributos
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LogAttribute : ActionFilterAttribute
    {

        private Object _args = null!;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                _args = context.ActionArguments.First().Value!;
            }
            catch { }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ILog _log = context.HttpContext.RequestServices.GetService<ILog>()!;
            var resultado = context.Result as ObjectResult;

            string idUsuario = context.HttpContext.Request.Headers["IdUsuario"].FirstOrDefault()?.Split(" ").Last()!;
            int idPrueba = int.Parse(context.HttpContext.Request.Headers["IdPrueba"].FirstOrDefault()?.Split(" ").Last() ?? "0");
            string ip = context.HttpContext.Connection.RemoteIpAddress?.ToString()!;
            string accion = context.HttpContext.Request.Path.Value!;

            string detalle = "";
            string tipo = "400";

            if (resultado != null && resultado!.Value is RespuestaDto)
            {
                RespuestaDto respuesta = (RespuestaDto)resultado!.Value!;
                detalle = respuesta.Detalle!;
                tipo = respuesta.Exito ? "200" : "400";
            }
            else
            {
                detalle = "";
            }

            if (context.Exception != null)
            {
                tipo = "500";
                detalle = context.Exception.Message + "\n" + context.Exception.StackTrace!.ToString();
            }

            if (idUsuario == null)
            {
                idUsuario = ObtenerElementoResult<string>(context, "IdUsuario")!;
                if (idUsuario == null)
                {
                    idUsuario = ObtenerElementoArgs<string>("IdUsuario")!;
                }
            }

            if (ip == null)
            {
                ip = ObtenerElementoResult<string>(context, "Ip")!;
                if (ip == null)
                {
                    ip = ObtenerElementoArgs<string>("Ip")!;
                }
            }

            if (idPrueba == 0)
            {
                idPrueba = ObtenerElementoResult<int>(context, "IdPrueba")!;
                if (idPrueba == 0)
                {
                    idPrueba = ObtenerElementoArgs<int>("IdPrueba")!;
                }
            }

            _log.Log(idUsuario, idPrueba, ip, accion, detalle, tipo);
        }

        private T? ObtenerElementoResult<T>(ActionExecutedContext context, string nombrePropiedad)
        {
            try
            {
                if (context.Result is ObjectResult objectResult)
                {
                    var resultObject = objectResult.Value;
                    if (resultObject != null)
                    {
                        var respuestaProperty = resultObject.GetType().GetProperty("Respuesta");
                        if (respuestaProperty != null)
                        {
                            var respuesta = respuestaProperty.GetValue(resultObject)!;
                            if (respuesta != null)
                            {
                                var idUsuarioProperty = respuesta.GetType().GetProperty(nombrePropiedad);
                                if (idUsuarioProperty != null)
                                {
                                    return (T)idUsuarioProperty.GetValue(respuesta)!;
                                }
                            }
                        }
                    }
                }
                return default;
            }
            catch
            {
                return default;
            }
        }

        private T? ObtenerElementoArgs<T>(string nombrePropiedad)
        {
            try
            {
                if (_args != null)
                {
                    var idUsuarioProperty = _args.GetType().GetProperty(nombrePropiedad);
                    if (idUsuarioProperty != null)
                    {
                        return (T)idUsuarioProperty.GetValue(_args)!;
                    }
                }
                return default;
            }
            catch
            {
                return default;
            }
        }

    }
}