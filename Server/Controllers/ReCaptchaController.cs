using EnergyDistribution.Shared.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EnergyDistribution.Server.Controllers
{
    public class ReCaptchaController : Controller
    {

        private readonly string _secretKey;

        public ReCaptchaController(IConfiguration config)
        {
            _secretKey = config["ReCaptcha:SecretKey"];
        }

        [HttpPost]
        [Route("api/Google/Captcha")]
        public async Task<IActionResult> CaptchaValidacion([FromBody] RespuestaCatpcha args)
        {
            //string metodoAPI = "API Google Captcha";
            try
            {
                var handler = new HttpClientHandler();
                #if DEBUG
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                #endif
                var httpClient = new HttpClient(handler);
                httpClient.Timeout = TimeSpan.FromMinutes(2);
                var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("secret", _secretKey), new KeyValuePair<string, string>("response", args.response!) });

                var response = await httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify", content);

                var respuestaJson = await response.Content.ReadAsStringAsync();
                var respuesta = JsonConvert.DeserializeObject<GoogleRespo>(respuestaJson)!;
                //ULog.Accion("", "", metodoAPI, $"Repuesta API Correcta - Token: {args.response}");
                return StatusCode(StatusCodes.Status200OK, respuesta);
            }
            catch (Exception error)
            {
                //ULog.Error("", "", metodoAPI, error.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, null);
            }

        }

    }
}
