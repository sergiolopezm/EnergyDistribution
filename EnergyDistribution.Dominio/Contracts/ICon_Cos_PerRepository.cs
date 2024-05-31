using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Domain.Contracts
{
    public interface  ICon_Cos_PerRepository
    {
        public RespuestaDto ExcelGeneral3Hojas(ExcelArgsDto args);
    }
}
