using EnergyDistribution.Shared.GeneralDTO;
using EnergyDistribution.Shared.InDTO;

namespace EnergyDistribution.Domain.Contracts
{
    public interface IUsuarioRepository
    {

        public RespuestaDto AutenticarUsuario(UsuarioIn args, string ip);

    }
}
