using EnergyDistribution.Shared.InDTO;
using System.Net.Http.Json;

namespace EnergyDistribution.Client.Services
{
    public class ImpresionDirecta
    {

        public async Task ImprimirPdfZebra(string PdfBase64)
        {
            HttpClient httpClientImp = new HttpClient();
            var impresion = new ImpresionIn
            {
                contenido = PdfBase64,
                alias = "SITRAC_ROTLIBROSCR"
            };
            var response = await httpClientImp.PostAsJsonAsync("http://localhost:8085/svcwebpdf/imprimirbase64", impresion);
        }

    }
}
