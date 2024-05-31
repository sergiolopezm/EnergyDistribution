using Microsoft.AspNetCore.Components.Authorization;
using EnergyDistribution.Client.Shared;
using EnergyDistribution.Shared.GeneralDTO;
using EnergyDistribution.Shared.OutDTO;
using System.Net.Http;
using System;
using System.Net.Http.Json;

namespace EnergyDistribution.Client.Services
{

    public class Api
    {

        public Modal Modal { get; set; } = new Modal();
        public Loading Loading { get; set; } = new Loading();

        private readonly HttpClient _httpClient;
        private readonly Autenticacion _autenticacion;
        private readonly string _mensajeError = "Error";
        private readonly string _detalleError = "Se ha presentado un error al acceder al servicio. Por favor, intente nuevamente";

        public Api(HttpClient httpClient, AuthenticationStateProvider autenticacion)
        {
            _httpClient = httpClient;
            _autenticacion = (Autenticacion)autenticacion;
        }

        public async Task<RespuestaClassDto<TOut>>? GetApiAcceso<TOut>(string ruta, bool verRespuestaCorrecta = false, int timeOut = 2)
        {
            try
            {
                Loading.Open();
                using (var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(timeOut)))
                {
                    HttpHeadersAcceso();
                    var httpResponse = await _httpClient.GetAsync(ruta);
                    var respuesta = await httpResponse.Content.ReadFromJsonAsync<RespuestaClassDto<TOut>>();
                    VerificarRespuestaHttp<TOut>(httpResponse!, respuesta!, verRespuestaCorrecta);
                    return respuesta!;
                }
            }
            catch (Exception error)
            {
                Modal!.OpenModalError(_mensajeError, _detalleError);
                Console.WriteLine(error.ToString());
                return default!;
            }
            finally
            {
                Loading.Close();
            }
        }

        public async Task<RespuestaClassDto<TOut>>? PostApiAcceso<TOut>(string ruta, object args, bool verRespuestaCorrecta = false, int timeOut = 2)
        {
            try
            {
                Loading.Open();
                using (var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(timeOut)))
                {
                    HttpHeadersAcceso();
                    var httpResponse = await _httpClient.PostAsJsonAsync(ruta, args, timeoutCancellationTokenSource.Token);
                    var respuesta = await httpResponse.Content.ReadFromJsonAsync<RespuestaClassDto<TOut>>();
                    VerificarRespuestaHttp<TOut>(httpResponse!, respuesta!, verRespuestaCorrecta);
                    return respuesta!;
                }
            }
            catch (Exception error)
            {
                Modal!.OpenModalError(_mensajeError, _detalleError);
                Console.WriteLine(error.ToString());
                return default!;
            }
            finally
            {
                Loading.Close();
            }

        }


        public async Task<RespuestaClassDto<TOut>>? GetApiAuth<TOut>(string ruta, bool verRespuestaCorrecta = false, int timeOut = 2)
        {
            try
            {
                Loading.Open();
                using (var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(timeOut)))
                {
                    await HttpHeadersAuth();
                    var httpResponse = await _httpClient.GetAsync(ruta);
                    var respuesta = await httpResponse.Content.ReadFromJsonAsync<RespuestaClassDto<TOut>>();
                    await VerificarRespuestaHttpAuth<TOut>(httpResponse!, respuesta!, verRespuestaCorrecta);
                    return respuesta!;
                }
            }
            catch (Exception error)
            {
                Modal!.OpenModalError(_mensajeError, _detalleError);
                Console.WriteLine(error.ToString());
                return default!;
            }
            finally
            {
                Loading.Close();
            }
        }

        public async Task<RespuestaClassDto<TOut>>? PostApiAuth<TOut>(string ruta, object args, bool verRespuestaCorrecta = false, int timeOut = 2)
        {
            try
            {
                Loading.Open();
                using (var timeoutCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(timeOut)))
                {
                    await HttpHeadersAuth();
                    var httpResponse = await _httpClient.PostAsJsonAsync(ruta, args);
                    var respuesta = await httpResponse.Content.ReadFromJsonAsync<RespuestaClassDto<TOut>>();
                    await VerificarRespuestaHttpAuth<TOut>(httpResponse!, respuesta!, verRespuestaCorrecta);
                    return respuesta!;
                }
            }
            catch (Exception error)
            {
                Modal!.OpenModalError(_mensajeError, _detalleError);
                Console.WriteLine(error.ToString());
                return default!;
            }
            finally
            {
                Loading.Close();
            }
        }

        private async Task HttpHeadersAuth()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            UsuarioOut usuario = await _autenticacion.ObtenerSesionUsuario();
            _httpClient.DefaultRequestHeaders.Add("IdUsuario", usuario.IdUsuario);
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + usuario.Token);
            _httpClient.DefaultRequestHeaders.Add("Sitio", "EnergyDistribution");
            _httpClient.DefaultRequestHeaders.Add("Clave", "12345");
        }

        private void HttpHeadersAcceso()
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Sitio", "EnergyDistribution");
            _httpClient.DefaultRequestHeaders.Add("Clave", "12345");
        }

        private void VerificarRespuestaHttp<TOut>(HttpResponseMessage response, RespuestaClassDto<TOut> respuesta, bool verRespuestaCorrecta = false)
        {
            if (respuesta.Resultado != null && respuesta!.Exito == true)
            {
                if (verRespuestaCorrecta)
                {
                    Modal!.OpenModalSuccess(respuesta.Mensaje!, respuesta.Detalle!);
                }
            }
            else
            {
                Modal!.OpenModalError(respuesta.Mensaje!, respuesta.Detalle!);
            }
        }

        private async Task VerificarRespuestaHttpAuth<TOut>(HttpResponseMessage response, RespuestaClassDto<TOut> respuesta, bool verRespuestaCorrecta = false)
        {
            if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
            {
                VerificarRespuestaHttp(response, respuesta, verRespuestaCorrecta);
            }
            else
            {
                Modal!.OpenModalError(respuesta.Mensaje!, respuesta.Detalle!);
                await _autenticacion.ActualizarEstadoAutenticion(null!);
            }
        }

    }
}
