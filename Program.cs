using restaurante_web_app.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace restaurante_web_app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();

            //añadir la cadena de conexion de la base de datos
            builder.Services.AddDbContext<GoeatContext>(opt => opt.UseNpgsql
            (builder.Configuration.GetConnectionString("cadenaPgSQL")));

            //ignorar las referecias ciclicas de los resultados JSON
            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            //añadir las reglas de CORS para permitir el uso de la API en cualquier dominio
            var reglasCors = "ReglasCors";
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy(name: reglasCors, builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
            }

            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            //activar las reglas CORS que hemos creado
            app.UseCors(reglasCors);

            app.Run();
        }
    }
}