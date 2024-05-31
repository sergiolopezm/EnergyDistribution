using System;
using System.Collections.Generic;

namespace EnergyDistribution.Infraestructure
{
    public partial class Tramo
    {
        public Tramo()
        {
            Consumos = new HashSet<Consumo>();
            Costos = new HashSet<Costo>();
            Perdida = new HashSet<Perdidum>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Consumo> Consumos { get; set; }
        public virtual ICollection<Costo> Costos { get; set; }
        public virtual ICollection<Perdidum> Perdida { get; set; }
    }
}
