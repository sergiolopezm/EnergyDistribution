using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Domain.Contracts
{
    public interface IReporte2HistoricoRepository
    {
        public RespuestaDto HistoricoConsumoCliente(ReportesFechasDto parametros);
    }
}
