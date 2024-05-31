using Blazored.LocalStorage;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using EnergyDistribution.Shared.OutDTO;

namespace EnergyDistribution.Client.Services
{
    public class Autenticacion: AuthenticationStateProvider
    {

        private readonly ILocalStorageService _localStorage;
        private readonly ClaimsPrincipal _sinimfo = new ClaimsPrincipal(new ClaimsIdentity());

        public Autenticacion(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task ActualizarEstadoAutenticion(UsuarioOut? usuario)
        {
            ClaimsPrincipal claimsPrincipal;

            if (usuario != null)
            {
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario!),
                    new Claim(ClaimTypes.Role, usuario.Rol!),
                    //new Claim("Prueba", usuario.NombrePrueba!)
                }, "JwtAuth"));
                await _localStorage.GuardarStorage("sesionUsuario", usuario);
            }
            else
            {
                claimsPrincipal = _sinimfo;
                await _localStorage.RemoveItemAsync("sesionUsuario");
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task<UsuarioOut> ObtenerSesionUsuario()
        {
            var sesion = await _localStorage.ObtenerStorage<UsuarioOut>("sesionUsuario");
            if (sesion == null)
            {
                await ActualizarEstadoAutenticion(null);
            }
            return sesion!;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var sesionUsuario1 = await _localStorage.ObtenerStorage<UsuarioOut>("sesionUsuario");

            if (sesionUsuario1 == null)
                return await Task.FromResult(new AuthenticationState(_sinimfo));

            var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name,sesionUsuario1.NombreUsuario!),
            }, "JwtAuth"));

            return await Task.FromResult(new AuthenticationState(claimPrincipal));
        }
    
    }

}
