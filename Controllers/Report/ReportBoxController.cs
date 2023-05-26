using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Data.DTOs;
using restaurante_web_app.Models;
using System.Data;

namespace restaurante_web_app.Controllers.Report
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportBoxController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public ReportBoxController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("daily")]
        public IActionResult GenerateDailyReport([FromQuery] DateTime fecha)
        {

            DateOnly currentDate = DateOnly.FromDateTime(fecha);

            CajaDiaria cajaDiaria = _dbContext.CajaDiaria.FirstOrDefault(c => c.Fecha == currentDate);

            if (cajaDiaria == null)
            {
                return NotFound("No se encontró la caja diaria para la fecha especificada.");
            }

            var movimientosCaja = (from mc in _dbContext.MovimientoCajas
                                   where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
                                   select new
                                   {
                                       mc.IdMovimiento,
                                       mc.IdTipoMovimiento,
                                       mc.Concepto,
                                       mc.Total,
                                       Ventas = (from vmc in _dbContext.VentaMovimientoCajas
                                                 join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
                                                 where vmc.IdMovimientoCaja == mc.IdMovimiento
                                                 select new
                                                 {
                                                     v.IdVenta,
                                                     v.NumeroComanda,
                                                     v.Fecha,
                                                     v.Total,
                                                     v.IdMesero,
                                                     v.IdCliente
                                                 }).ToList(),
                                       Gastos = (from gmc in _dbContext.GastosMovimientoCajas
                                                 join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
                                                 where gmc.IdMovimientoCaja == mc.IdMovimiento
                                                 select new
                                                 {
                                                     g.IdGasto,
                                                     g.NumeroDocumento,
                                                     g.Fecha,
                                                     g.Concepto,
                                                     g.Total,
                                                     g.IdProveedor
                                                 }).ToList()
                                   }).ToList();

            decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
            decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

            // Realizar cálculos solo si el estado de la caja es verdadero (true)
            //if (!cajaDiaria.Estado)
            //{
            //    cajaDiaria.SaldoInicial += totalVentas;
            //    totalGastos -= totalVentas;
            //}

            decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
            cajaDiaria.SaldoFinal = saldoFinal;

            var response = new
            {
                CajaDiaria = new
                {
                    cajaDiaria.IdCajaDiaria,
                    cajaDiaria.Fecha,
                    cajaDiaria.SaldoInicial,
                    cajaDiaria.SaldoFinal,
                    cajaDiaria.Estado
                },
                MovimientosCaja = movimientosCaja,
                TotalVentas = totalVentas,
                TotalGastos = totalGastos
            };


            if (response == null)
            {
                return NotFound("No se encontraron datos para la fecha especificada.");
            }

            return Ok(response);

        }


        [Authorize(Roles = "Administrador")]
        [HttpGet("dailyweek")]
        public IActionResult weeBoxRepor()
        {

            DayOfWeek currentDayOfWeek = DateTime.Today.DayOfWeek;
            int daysUntilStartOfWeek = ((int)currentDayOfWeek - 1 + 7) % 7;
            DateTime startOfWeek = DateTime.Today.AddDays(-daysUntilStartOfWeek).Date;
            DateTime endOfWeek = startOfWeek.AddDays(7).Date;

            DateOnly startOfWeekDateOnly = DateOnly.FromDateTime(startOfWeek);
            DateOnly endOfWeekDateOnly = DateOnly.FromDateTime(endOfWeek);

            List<CajaDiaria> cajasSemanales = _dbContext.CajaDiaria
                .Where(c => c.Fecha >= startOfWeekDateOnly && c.Fecha <= endOfWeekDateOnly)
                .ToList();

            if (cajasSemanales.Count == 0)
            {
                return NotFound("No se encontraron cajas diarias para la semana actual.");
            }

            List<object> response = new List<object>();

            foreach (CajaDiaria cajaDiaria in cajasSemanales)
            {
                var movimientosCaja = (from mc in _dbContext.MovimientoCajas
                                       where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
                                       select new
                                       {
                                           mc.IdMovimiento,
                                           mc.IdTipoMovimiento,
                                           mc.Concepto,
                                           mc.Total,
                                           Ventas = (from vmc in _dbContext.VentaMovimientoCajas
                                                     join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
                                                     where vmc.IdMovimientoCaja == mc.IdMovimiento
                                                     select new
                                                     {
                                                         v.IdVenta,
                                                         v.NumeroComanda,
                                                         v.Fecha,
                                                         v.Total,
                                                         v.IdMesero,
                                                         v.IdCliente
                                                     }).ToList(),
                                           Gastos = (from gmc in _dbContext.GastosMovimientoCajas
                                                     join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
                                                     where gmc.IdMovimientoCaja == mc.IdMovimiento
                                                     select new
                                                     {
                                                         g.IdGasto,
                                                         g.NumeroDocumento,
                                                         g.Fecha,
                                                         g.Concepto,
                                                         g.Total,
                                                         g.IdProveedor
                                                     }).ToList()
                                       }).ToList();

                decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
                decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

                decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
                cajaDiaria.SaldoFinal = saldoFinal;

                
                var cajaResponse = new
                {
                    CajaDiaria = new
                    {
                        cajaDiaria.IdCajaDiaria,
                        cajaDiaria.Fecha,
                        cajaDiaria.SaldoInicial,
                        cajaDiaria.SaldoFinal,
                        cajaDiaria.Estado
                    },
                    MovimientosCaja = movimientosCaja,
                    TotalVentas = totalVentas,
                    TotalGastos = totalGastos,
                    //Tot = ingresoTotalSemana
                };

                response.Add(cajaResponse);
            }

            return Ok(response);

        }


    }
}
