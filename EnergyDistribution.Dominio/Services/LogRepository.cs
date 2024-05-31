using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Infraestructure;

namespace EnergyDistribution.Domain.Services
{
    public class LogRepository : ILog
    {
        
        private readonly DBContext _context;

        public LogRepository(DBContext context)
        {
            _context = context;
        }


        public void Accion(string? idUsuario, int idPrueba, string? ip, string? accion, string? detalle)
        {
            Task guardarLog = GuardarLogAsync(idUsuario, idPrueba, ip, accion, detalle, "200");
        }

        public void Error(string? idUsuario, int idPrueba, string? ip, string? accion, string? error)
        {
            Task guardarLog = GuardarLogAsync(idUsuario, idPrueba, ip, accion, error, "500");
        }

        public void Info(string? idUsuario, int idPrueba, string? ip, string? accion, string? detalle)
        {
            Task guardarLog = GuardarLogAsync(idUsuario, idPrueba, ip, accion, detalle, "400");
        }

        public void Log(string? idUsuario, int idPrueba, string? ip, string? accion, string? detalle, string tipo)
        {
            Task guardarLog = GuardarLogAsync(idUsuario, idPrueba, ip, accion, detalle, tipo);
        }

        private async Task GuardarLogAsync(string? idUsuario, int idPrueba, string? ip, string? accion, string? detalle, string tipo)
        {
            Log log = new Log();

            log.Fecha = DateTime.Now;
            if (idUsuario != null && idUsuario.Length <= 50)
            {
                log.IdUsuario = idUsuario;
            }
            log.Ip = ip;
            log.Accion = accion;
            log.Tipo = tipo;
            log.Detalle = detalle;


            Task guardarTxt = GuardarTxtAsync(log);
            _context.ChangeTracker.Clear();
            await _context.Logs.AddAsync(log);
            _context.SaveChanges();
        }

        private async Task GuardarTxtAsync(Log log)
        {
            string logFileName = "LOG_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string logFilePath = Path.Combine("Logs", logFileName);
            Directory.CreateDirectory("Logs");

            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                string Log = $"FECHA:\t\t{DateTime.Now}\n" +
                    $"TIPO:\t\t{log.Tipo}\n" +
                    $"USUARIO:\t{log.IdUsuario}\n" +
                    $"IP:\t\t\t{log.Ip}\nACCIÓN:\t\t{log.Accion}\n" +
                    $"DETALLE:\t{log.Detalle}\n" +
                    $"------------------------------------------------------------------------------------------";
                await sw.WriteLineAsync(Log);
            }
        }

    }
}
