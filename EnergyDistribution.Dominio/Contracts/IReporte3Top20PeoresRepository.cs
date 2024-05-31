using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Domain.Contracts
{
    public interface IReporte3Top20PeoresRepository
    {
        public RespuestaDto ObtenerTopPeoresTramos(ReportesFechasDto parametros);
    }
}
