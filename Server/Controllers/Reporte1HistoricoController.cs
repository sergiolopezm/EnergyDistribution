using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Shared.GeneralDTO;
using Microsoft.AspNetCore.Mvc;

namespace EnergyDistribution.Server.Controllers
{
    [Route("api/Historico/")]
    public class Reporte1HistoricoController : Controller
    {
       private readonly IReporte1HistoricoRepository _reporte1Historico;

        public Reporte1HistoricoController(IReporte1HistoricoRepository reporte1HistoricoRepository)
        {
            _reporte1Historico = reporte1HistoricoRepository;
        }

        [HttpGet]
        [Route("ConsumoTramos")]
        public IActionResult HistoricoConsumoTramos([FromBody] ReportesFechasDto parametros)
        {
            RespuestaDto ValidarExcelArticulo = _reporte1Historico.HistoricoConsumoTramos(parametros);
            return StatusCode(ValidarExcelArticulo.Exito ? 200 : 400, ValidarExcelArticulo);
        }
    }
}
