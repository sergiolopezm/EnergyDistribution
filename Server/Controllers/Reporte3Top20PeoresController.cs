using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Shared.GeneralDTO;
using Microsoft.AspNetCore.Mvc;

namespace EnergyDistribution.Server.Controllers
{
    [Route("api/Historico/")]
    public class Reporte3Top20PeoresController : Controller
    {
       private  readonly IReporte3Top20PeoresRepository _reporte3Top20Peores;

        public Reporte3Top20PeoresController(IReporte3Top20PeoresRepository reporte3Top20PeoresRepository)
        {
            _reporte3Top20Peores = reporte3Top20PeoresRepository;
        }

        [HttpGet]
        [Route("Top20PeoresTramos")]
        public IActionResult ObtenerTopPeoresTramos([FromBody] ReportesFechasDto parametros)
        {
            RespuestaDto ValidarExcelArticulo = _reporte3Top20Peores.ObtenerTopPeoresTramos(parametros);
            return StatusCode(ValidarExcelArticulo.Exito ? 200 : 400, ValidarExcelArticulo);
        }
    }
}
