using Microsoft.EntityFrameworkCore;
using EnergyDistribution.Domain.Contracts;
using EnergyDistribution.Domain.Services;
using EnergyDistribution.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Se Agrega Cadena de Conexion a base de Datos 
builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);
});


// Inyección de Dependencias  de Servicios/Contratos
builder.Services.AddScoped<IAccesoRepository, AccesoRepository>();
builder.Services.AddScoped<ILog, LogRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<ICon_Cos_PerRepository, Con_Cos_PerRepository>();
builder.Services.AddScoped<IReporte1HistoricoRepository, Reporte1HistoricoRepository>();
builder.Services.AddScoped<IReporte2HistoricoRepository, Reporte2HistoricoRepository>();
builder.Services.AddScoped<IReporte3Top20PeoresRepository, Reporte3Top20PeoresRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
