using Microsoft.Extensions.Configuration;
using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Infraestructure;
using EnergyDistribution.Shared.GeneralDTO;
using System.Text;

namespace EnergyDistribution.Domain.Services
{
    public partial class TokenRepository : ITokenRepository
    {
        private readonly DBContext _context;
        private readonly byte[] _keyBytes;
        private readonly int _tiempoExpiracion;
        private readonly int _tiempoExpiracionBD;

        public TokenRepository(IConfiguration config, DBContext context)
        {
            _context = context;
            string keyJwt = config.GetSection("JwtSettings")["Key"];
            _keyBytes = Encoding.UTF8.GetBytes(keyJwt);
            _tiempoExpiracion = int.Parse(config.GetSection("JwtSettings")["TiempoExpiracionMinutos"]);
            _tiempoExpiracionBD = int.Parse(config.GetSection("JwtSettings")["TiempoExpiracionBDMinutos"]);
        }

        public string GenerarToken(Usuario usuario, string ip)
        {
            RemoverTokensExpirados(usuario);
            string token = CrearTokenUsuario(usuario, ip);
            
            GuardarTokenBD(token, usuario, ip);
            return token;
        }

        public bool CancelarToken(string Token)
        {
            throw new NotImplementedException();
        }

        public object ObtenerInformacionToken(string Token)
        {
            throw new NotImplementedException();
        }

        public ValidoDTO EsValido(string idToken, string idUsuario, string ip)
        {
            ValidoDTO validarToken = ValidarTokenEnSistema(idToken, idUsuario, ip);
            if(!validarToken.EsValido)
            {
                return validarToken;
            }
            return  ValidarTokenEnBD(idToken);
        }

        public void AumentarTiempoExpiracion(string idToken)
        {
            Token token = _context.Tokens.FirstOrDefault(t => t.IdToken == idToken)!;
            if(token != null)
            {
                token.FechaExpiracion = DateTime.Now.AddMinutes(_tiempoExpiracionBD);
                _context.SaveChanges();
            }
        }

    }
}
