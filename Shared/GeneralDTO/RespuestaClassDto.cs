namespace EnergyDistribution.Shared.GeneralDTO
{
    public class RespuestaClassDto<T>
    {

        public bool Exito { get; set; }

        public string? Mensaje { get; set; }

        public string? Detalle { get; set; }

        public T? Resultado { get; set; }

    }
}
