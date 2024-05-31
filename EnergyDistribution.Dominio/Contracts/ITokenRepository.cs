using EnergyDistribution.Infraestructure;
using EnergyDistribution.Shared.GeneralDTO;

namespace EnergyDistribution.Domain.Contracts
{
    public interface ITokenRepository
    {

        public string GenerarToken(Usuario usuario, string ip);

        public bool CancelarToken(string Token);

        public Object ObtenerInformacionToken(string Token);

        public ValidoDTO EsValido(string idToken, string idUsuario, string ip);

        public void AumentarTiempoExpiracion(string token);

    }
}
