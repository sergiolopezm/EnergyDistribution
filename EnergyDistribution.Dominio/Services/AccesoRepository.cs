using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Infraestructure;

namespace EnergyDistribution.Domain.Services
{
    public class AccesoRepository : IAccesoRepository
    {
        
        private readonly DBContext _context;
        
        public AccesoRepository(DBContext context)
        {
            _context = context;
        }


        public bool ValidarAcceso(string sitio, string contraseña)
        {
            if(_context.Accesos.Where(a => a.Sitio == sitio && a.Contraseña == contraseña).Any())
            {
                return true;
            }
            return false;
        }
    }
}
