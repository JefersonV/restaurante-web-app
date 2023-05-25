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
    public class CajaController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public CajaController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{caja}")]
        public async Task<JsonResult> GetCaja()
        {
            // Obtener la fecha actual
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);

            var fechaAnterior = fechaActual.AddDays(-1);
            var cajaAnterior = await _dbContext.CajaDiaria
                .OrderByDescending(c => c.Fecha)
                .FirstOrDefaultAsync(c => c.Fecha == fechaAnterior);

            if (cajaAnterior == null)
            {
                return new JsonResult( new { cajaAnterior = 0 });
            }

            return new JsonResult(new { cajaAnterior = cajaAnterior.SaldoFinal });
        }


        [HttpGet]
        public async Task<IEnumerable<CajaDtoOut>> GetAll()
        {
            var cajasDiarias = await _dbContext.CajaDiaria
                .OrderByDescending(c => c.IdCajaDiaria).ToListAsync();
            var cajasDtoOut = new List<CajaDtoOut>();

            foreach (var cajaDiaria in cajasDiarias)
            {
                decimal? totalVentas = await GetTotalByMonth("Ventas", cajaDiaria.Fecha);
                decimal? totalGastos = await GetTotalByMonth("Gastos", cajaDiaria.Fecha);

                var cajaDtoOut = new CajaDtoOut
                {
                    IdCajaDiaria = cajaDiaria.IdCajaDiaria,
                    Fecha = cajaDiaria.Fecha,
                    Estado = cajaDiaria.Estado,
                    SaldoInicial = cajaDiaria.SaldoInicial,
                    Ingreso = totalVentas,
                    Egreso = totalGastos,
                    Caja = cajaDiaria.SaldoInicial + (totalVentas ?? 0) - (totalGastos ?? 0),
                    Entrega = cajaDiaria.SaldoFinal,
                    SaldoBruto = totalVentas,
                    Ganancia = (totalVentas ?? 0) - (totalGastos ?? 0)
                };

                cajasDtoOut.Add(cajaDtoOut);
            }

            return cajasDtoOut;
        }


        private async Task<decimal?> GetTotalByMonth(string tableName, DateOnly date)
        {
            decimal? total = null;
            if (tableName == "Ventas")
            {
                total = await _dbContext.Ventas
                    .Where(v => v.Fecha.Value.Day == date.Day && v.Fecha.Value.Month == date.Month 
                    && v.Fecha.Value.Year == date.Year)
                    .SumAsync(v => v.Total);
            }
            else if (tableName == "Gastos")
            {
                total = await _dbContext.Gastos
                    .Where(g => g.Fecha.Value.Day == date.Day && g.Fecha.Value.Month == date.Month 
                    && g.Fecha.Value.Year == date.Year)
                    .SumAsync(g => g.Total);
            }
            return total;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CajaDtoIn cajaDtoIn)
        {
            // Obtener la fecha actual
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Now);

            // Obtener el saldo inicial ingresado
            decimal? saldoInicial = cajaDtoIn.SaldoInicial;

            // Obtener el saldo final de la caja del día anterior y sumarle el saldo inicial
            var fechaAnterior = fechaActual.AddDays(-1);
            var cajaAnterior = await _dbContext.CajaDiaria
                .OrderByDescending(c => c.Fecha)
                .FirstOrDefaultAsync(c => c.Fecha == fechaAnterior);

            decimal? saldoNuevo = cajaAnterior != null ? cajaAnterior.SaldoFinal + saldoInicial : saldoInicial;

            // Crear el nuevo objeto CajaDiaria
            var nuevaCajaDiaria = new CajaDiaria
            {
                Fecha = fechaActual,
                SaldoInicial = saldoNuevo,
                SaldoFinal = 0,
                Estado = true
            };

            // Agregar la nueva caja a la base de datos
            _dbContext.CajaDiaria.Add(nuevaCajaDiaria);
            await _dbContext.SaveChangesAsync();

            return Ok(); // O devuelve la respuesta deseada
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, CajaUpdateDto cajaUpdateDto)
        {
            var cajaDiaria = await _dbContext.CajaDiaria
                .FirstOrDefaultAsync(c => c.IdCajaDiaria == id);

            if (cajaDiaria == null)
            {
                return NotFound();
            }

            decimal? totalVentas = await GetTotalByMonth("Ventas", cajaDiaria.Fecha);
            decimal? totalGastos = await GetTotalByMonth("Gastos", cajaDiaria.Fecha);

            if (cajaDiaria == null)
            {
                return NotFound();
            }
            decimal? cajaActual = cajaDiaria.SaldoInicial + (totalVentas ?? 0) - (totalGastos ?? 0);
            if(cajaActual < cajaUpdateDto.CantidadSacar)
            {
                return BadRequest(new { mensaje = "La cantidad en caja no es sufciente" });
            }
            cajaDiaria.SaldoFinal = cajaActual - cajaUpdateDto.CantidadSacar;
            cajaDiaria.Estado = false;

            _dbContext.CajaDiaria.Update(cajaDiaria);
            await _dbContext.SaveChangesAsync();

            return Ok(); // O devuelve la respuesta deseada
        }
    }
}
