using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Infraestructure;
using EnergyDistribution.Shared.GeneralDTO;
using EnergyDistribution.Shared.InDTO;
using EnergyDistribution.Shared.OutDTO;
using EnergyDistribution.Util;

namespace EnergyDistribution.Domain.Services
{
    public partial class UsuarioRepository : IUsuarioRepository
    {
        
        private readonly DBContext _context;
        private readonly ITokenRepository _token;

        public UsuarioRepository(DBContext context, ITokenRepository token)
        {
            _context = context;
            _token = token;
        }

        public RespuestaDto AutenticarUsuario(UsuarioIn args, string ip)
        {
            Usuario usuario = _context.Usuarios.Where(u => u.Nombre == args.NombreUsuario! && u.Contraseña == args.Contraseña!).FirstOrDefault()!;
            
            if (usuario == null)
            {
                return RespuestaDto.ParametrosIncorrectos("Sesion fallida", "El usuario o la contraseña son incorrectos");
            }
            
            if(usuario.Contraseña != args.Contraseña)
            {
                return RespuestaDto.ParametrosIncorrectos("Sesion fallida", "El usuario o la contraseña son incorrectos");
            }

            if(usuario.Activo == false)
            {
                return RespuestaDto.ParametrosIncorrectos("Sesion fallida", "El usuario no se encuentra activo");
            }
            
            string token = _token.GenerarToken(usuario, ip);

            UsuarioOut usuarioOutDTO = Maping.Convertir<Usuario, UsuarioOut>(usuario);
            usuarioOutDTO.Token = token;

            return new RespuestaDto
            { 
                Exito = true,
                Mensaje = "Usuario autenticado",
                Detalle = $"El usuario {usuarioOutDTO.NombreUsuario} se ha autenticado correctamente.",
                Resultado = usuarioOutDTO
            };
        }

    }

}