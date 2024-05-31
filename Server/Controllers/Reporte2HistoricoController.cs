using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Shared.GeneralDTO;
using Microsoft.AspNetCore.Mvc;

namespace EnergyDistribution.Server.Controllers
{
    [Route("api/Historico")]
    public class Reporte2HistoricoController : Controller
    {
        private readonly IReporte2HistoricoRepository _reporte1Historico;

        public Reporte2HistoricoController(IReporte2HistoricoRepository reporte2HistoricoRepository)
        {
            _reporte1Historico = reporte2HistoricoRepository;
        }

        [HttpGet]
        [Route("ConsumoCliente")]
        public IActionResult HistoricoConsumoCliente([FromBody] ReportesFechasDto parametros)
        {
            RespuestaDto ValidarExcelArticulo = _reporte1Historico.HistoricoConsumoCliente(parametros);
            return StatusCode(ValidarExcelArticulo.Exito ? 200 : 400, ValidarExcelArticulo);
        }
    }
}
