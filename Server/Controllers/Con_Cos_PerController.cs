using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Shared.GeneralDTO;
using Microsoft.AspNetCore.Mvc;

namespace EnergyDistribution.Server.Controllers
{
    [Route("api/Insertar/")]
    public partial class Con_Cos_PerController : Controller
    {
       private readonly ICon_Cos_PerRepository _consumoRepository;

        public Con_Cos_PerController(ICon_Cos_PerRepository consumoRepository)
        {
            _consumoRepository = consumoRepository;
        }

        [HttpPost]
        [Route("ExcelGeneral")]
        public IActionResult ExcelGeneral3Hojas([FromBody]ExcelArgsDto args)
        {
            RespuestaDto resultado = _consumoRepository.ExcelGeneral3Hojas(args);
            return StatusCode(resultado.Exito ? 200 : 400, resultado);
        }
    }
}
