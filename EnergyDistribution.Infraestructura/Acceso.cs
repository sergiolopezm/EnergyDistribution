﻿using System;
using System.Collections.Generic;

namespace EnergyDistribution.Infraestructure
{
    public partial class Acceso
    {
        public int IdAcceso { get; set; }
        public string? Sitio { get; set; }
        public string? Contraseña { get; set; }
    }
}
