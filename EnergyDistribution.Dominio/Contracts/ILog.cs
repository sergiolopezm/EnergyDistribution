namespace EnergyDistribution.Domain.Contracts
{
    public interface ILog
    {
        public void Accion(string? idUsuario, int idPrueba, string? ip, string? accion, string? detalle);

        public void Info(string? idUsuario, int idPrueba, string? ip, string? accion, string? error);

        public void Error(string? idUsuario, int idPrueba, string? ip, string? accion, string? error);

        public void Log(string? idUsuario, int idPrueba, string? ip, string? accion, string? detalle, string tipo);

    }
}
