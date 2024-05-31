using System;
using System.Collections.Generic;

namespace EnergyDistribution.Infraestructure
{
    public partial class Usuario
    {
        public Usuario()
        {
            TokenExpirados = new HashSet<TokenExpirado>();
            Tokens = new HashSet<Token>();
        }

        public string IdUsuario { get; set; } = null!;
        public string? NombreUsuario { get; set; }
        public string? Contraseña { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Rol { get; set; }
        public bool? Activo { get; set; }

        public virtual ICollection<TokenExpirado> TokenExpirados { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
