using EnergyDistribution.Shared.ReCaptcha;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace EnergyDistribution.Client.Services
{
    public class GoogleReCaptcha
    {

        private readonly HttpClient _httpClient;

        public GoogleReCaptcha(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public virtual async Task<bool> reVerify(string _Token)
        {
            var respuestaToken = new RespuestaCatpcha
            {
                response = _Token
            };
            var response = await _httpClient.PostAsJsonAsync<RespuestaCatpcha>("api/Google/Captcha", respuestaToken);

            var jsonString = await response.Content.ReadAsStringAsync();
            var capresponse = JsonConvert.DeserializeObject<GoogleRespo>(jsonString);
            return capresponse!.success;
        }

    }

}
