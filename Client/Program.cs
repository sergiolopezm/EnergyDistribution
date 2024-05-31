using Blazored.LocalStorage;
using EnergyDistribution.Client;
using EnergyDistribution.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, Autenticacion>();
builder.Services.AddScoped<GoogleReCaptcha>();
builder.Services.AddScoped<Api>();
builder.Services.AddScoped<ImpresionDirecta>();
builder.Services.AddAuthorizationCore();


await builder.Build().RunAsync();
