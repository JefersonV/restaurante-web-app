using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Data.DTOs;
using restaurante_web_app.Models;


namespace restaurante_web_app.Controllers
{
    //añadir las reglas de cors dentro de cada controlador
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public DashboardController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("total")]
        public async Task<JsonResult> GetTotalByMonth()
        {
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);
            DateOnly fechaMesAnterior = fechaActual.AddMonths(-1);
            //DateOnly fechaActual = new DateOnly(2023, 4, 14);
            //DateOnly fechaMesAnterior = new DateOnly(2023, 3, 14);

            var cajaDiaria = await _dbContext.CajaDiaria
                .OrderByDescending(c => c.Fecha)
                .FirstOrDefaultAsync(c => c.Fecha == fechaActual);

            decimal? totalVentas = await GetTotalByMonth("Ventas", fechaActual);
            decimal? totalVentasMesAnterior = await GetTotalByMonth("Ventas", fechaMesAnterior);

            decimal? totalGastos = await GetTotalByMonth("Gastos", fechaActual);
            decimal? totalGastosMesAnterior = await GetTotalByMonth("Gastos", fechaMesAnterior);

            decimal? ganancia = CalculateGanancia(totalVentas, totalGastos);
            decimal? gananciaMesAnterior = CalculateGanancia(totalVentasMesAnterior, totalGastosMesAnterior);

            decimal? porcentajeCambioVentas = CalculatePorcentajeCambio(totalVentas, totalVentasMesAnterior);
            decimal? porcentajeCambioGastos = CalculatePorcentajeCambio(totalGastos, totalGastosMesAnterior);
            decimal? porcentajeCambioGanancia = CalculatePorcentajeCambio(ganancia, gananciaMesAnterior);

            decimal? cajaActual = 0;

            if (cajaDiaria != null && cajaDiaria.Estado == true)
            {
                cajaActual = cajaDiaria.SaldoInicial + (totalVentas ?? 0) - (totalGastos ?? 0);
            }

            var data = new
            {
                totalVentasMesActual = totalVentas,
                porcentajeCambioVentas,
                totalGastosMesActual = totalGastos,
                porcentajeCambioGastos,
                ganancia,
                porcentajeCambioGanancia,
                cajaActual,
            };

            return new JsonResult(data);
        }

        private async Task<decimal?> GetTotalByMonth(string tableName, DateOnly date)
        {
            decimal? total = null;
            if (tableName == "Ventas")
            {
                total = await _dbContext.Ventas
                    .Where(v => v.Fecha.Value.Month == date.Month && v.Fecha.Value.Year == date.Year)
                    .SumAsync(v => v.Total);
            }
            else if (tableName == "Gastos")
            {
                total = await _dbContext.Gastos
                    .Where(g => g.Fecha.Value.Month == date.Month && g.Fecha.Value.Year == date.Year)
                    .SumAsync(g => g.Total);
            }
            return total;
        }

        private decimal? CalculateGanancia(decimal? ventas, decimal? gastos)
        {
            if (ventas.HasValue && gastos.HasValue)
            {
                return ventas.Value - gastos.Value;
            }
            return null;
        }

        private decimal? CalculatePorcentajeCambio(decimal? valorActual, decimal? valorAnterior)
        {
            if (valorActual.HasValue && valorAnterior.HasValue && valorAnterior.Value != 0)
            {
                decimal cambioPorcentual = ((valorActual.Value - valorAnterior.Value) / valorAnterior.Value) * 100;
                return Math.Round(cambioPorcentual, 2);
            }
            return null;
        }

        private async Task<decimal?> GetTotalByMonth(string tableName, int year, int month)
        {
            decimal? total = null;
            if (tableName == "Ventas")
            {
                total = await _dbContext.Ventas
                    .Where(v => v.Fecha.Value.Month == month && v.Fecha.Value.Year == year)
                    .SumAsync(v => v.Total);
            }
            else if (tableName == "Gastos")
            {
                total = await _dbContext.Gastos
                    .Where(g => g.Fecha.Value.Month == month && g.Fecha.Value.Year == year)
                    .SumAsync(g => g.Total);
            }
            return total;
        }

        [HttpGet("ganancias")]
        public async Task<JsonResult> GetGananciasByMonth()
        {
            var year = DateTime.Now.Year;
            var meses = new[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", 
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            var data = new List<object>();

            for (int month = 1; month <= 12; month++)
            {
                decimal? totalVentas = await GetTotalByMonth("Ventas", year, month);
                decimal? totalGastos = await GetTotalByMonth("Gastos", year, month);
                decimal? ganancia = totalVentas - totalGastos;
                data.Add(new { mes = meses[month - 1], total = ganancia ?? 0 });
            }

            return new JsonResult(data);
        }


    }
}
