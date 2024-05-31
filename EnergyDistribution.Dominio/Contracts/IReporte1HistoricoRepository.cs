using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Domain.Contracts
{
    public interface IReporte1HistoricoRepository
    {
        public RespuestaDto HistoricoConsumoTramos(ReportesFechasDto parametros);
    }
}
