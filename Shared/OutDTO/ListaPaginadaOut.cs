namespace EnergyDistribution.Shared.OutDTO
{
    public class ListaPaginadaOut<T> where T : class
    {
        
        public int pagina { get; set; }

        public int totalPaginas { get; set; }

        public int totalRegistros { get; set; }

        public List<T>? lista { get; set; }

    }
}
