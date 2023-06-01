using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Data.DTOs;
using restaurante_web_app.Models;
using System.Data;
using System.Globalization;

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
        public IActionResult GenerateDailyReport( DateTime fecha)
        {

            DateOnly currentDate = DateOnly.FromDateTime(fecha);

            CajaDiaria cajaDiaria = _dbContext.CajaDiaria.FirstOrDefault(c => c.Fecha == currentDate);

            if (cajaDiaria == null)
            {
                return NotFound("Caja diaria para la fecha especificada no encontrada.");
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
                return NotFound("Response encontraron datos para la fecha especificada.");
            }
                //if (movimientosCaja == null)
                //{
                //    return NotFound("Movimientocajas NO hay daros");
                //}

                return Ok(response);

        }


        //[Authorize(Roles = "Administrador")]
        //[HttpGet("dailyweek")]
        //public IActionResult weeBoxRepor()
        //{

        //    DayOfWeek currentDayOfWeek = DateTime.Today.DayOfWeek;
        //    int daysUntilStartOfWeek = ((int)currentDayOfWeek - 1 + 7) % 7;
        //    DateTime startOfWeek = DateTime.Today.AddDays(-daysUntilStartOfWeek).Date;
        //    DateTime endOfWeek = startOfWeek.AddDays(7).Date;

        //    DateOnly startOfWeekDateOnly = DateOnly.FromDateTime(startOfWeek);
        //    DateOnly endOfWeekDateOnly = DateOnly.FromDateTime(endOfWeek);

        //    List<CajaDiaria> cajasSemanales = _dbContext.CajaDiaria
        //        .Where(c => c.Fecha >= startOfWeekDateOnly && c.Fecha <= endOfWeekDateOnly)
        //        .ToList();

        //    if (cajasSemanales.Count == 0)
        //    {
        //        return NotFound("No se encontraron cajas diarias para la semana actual.");
        //    }

        //    List<object> response = new List<object>();

        //    foreach (CajaDiaria cajaDiaria in cajasSemanales)
        //    {
        //        var movimientosCaja = (from mc in _dbContext.MovimientoCajas
        //                               where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
        //                               select new
        //                               {
        //                                   mc.IdMovimiento,
        //                                   mc.IdTipoMovimiento,
        //                                   mc.Concepto,
        //                                   mc.Total,
        //                                   Ventas = (from vmc in _dbContext.VentaMovimientoCajas
        //                                             join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
        //                                             where vmc.IdMovimientoCaja == mc.IdMovimiento
        //                                             select new
        //                                             {
        //                                                 v.IdVenta,
        //                                                 v.NumeroComanda,
        //                                                 v.Fecha,
        //                                                 v.Total,
        //                                                 v.IdMesero,
        //                                                 v.IdCliente
        //                                             }).ToList(),
        //                                   Gastos = (from gmc in _dbContext.GastosMovimientoCajas
        //                                             join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
        //                                             where gmc.IdMovimientoCaja == mc.IdMovimiento
        //                                             select new
        //                                             {
        //                                                 g.IdGasto,
        //                                                 g.NumeroDocumento,
        //                                                 g.Fecha,
        //                                                 g.Concepto,
        //                                                 g.Total,
        //                                                 g.IdProveedor
        //                                             }).ToList()
        //                               }).ToList();

        //        decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
        //        decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

        //        decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
        //        cajaDiaria.SaldoFinal = saldoFinal;

                
        //        var cajaResponse = new
        //        {
        //            CajaDiaria = new
        //            {
        //                cajaDiaria.IdCajaDiaria,
        //                cajaDiaria.Fecha,
        //                cajaDiaria.SaldoInicial,
        //                cajaDiaria.SaldoFinal,
        //                cajaDiaria.Estado
        //            },
        //            MovimientosCaja = movimientosCaja,
        //            TotalVentas = totalVentas,
        //            TotalGastos = totalGastos,
        //            //Tot = ingresoTotalSemana
        //        };

        //        response.Add(cajaResponse);
        //    }

        //    return Ok(response);

        //}









        //****************************************
        //[Authorize(Roles = "Administrador")]
        //[HttpGet("dia")]
        //public IActionResult GenerateDailyRepo([FromQuery] DateTime fecha)
        //{
        //    DateOnly currentDate = DateOnly.FromDateTime(fecha);

        //    CajaDiaria cajaDiaria = _dbContext.CajaDiaria.FirstOrDefault(c => c.Fecha == currentDate);

        //    if (cajaDiaria == null)
        //    {
        //        return NotFound("No se encontró la caja diaria para la fecha especificada.");
        //    }

        //    if (!cajaDiaria.Estado)
        //    {
        //        return BadRequest("El estado de la caja es 'false'. No se pueden mostrar ni agregar los datos.");
        //    }

        //    var movimientosCaja = (from mc in _dbContext.MovimientoCajas
        //                           where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
        //                           select new
        //                           {
        //                               mc.IdMovimiento,
        //                               mc.IdTipoMovimiento,
        //                               mc.Concepto,
        //                               mc.Total,
        //                               Ventas = (from vmc in _dbContext.VentaMovimientoCajas
        //                                         join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
        //                                         where vmc.IdMovimientoCaja == mc.IdMovimiento
        //                                         select new
        //                                         {
        //                                             v.IdVenta,
        //                                             v.NumeroComanda,
        //                                             v.Fecha,
        //                                             v.Total,
        //                                             v.IdMesero,
        //                                             v.IdCliente
        //                                         }).ToList(),
        //                               Gastos = (from gmc in _dbContext.GastosMovimientoCajas
        //                                         join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
        //                                         where gmc.IdMovimientoCaja == mc.IdMovimiento
        //                                         select new
        //                                         {
        //                                             g.IdGasto,
        //                                             g.NumeroDocumento,
        //                                             g.Fecha,
        //                                             g.Concepto,
        //                                             g.Total,
        //                                             g.IdProveedor
        //                                         }).ToList()
        //                           }).ToList();

        //    decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
        //    decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

        //    decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
        //    cajaDiaria.SaldoFinal = saldoFinal;

        //    var response = new
        //    {
        //        CajaDiaria = new
        //        {
        //            cajaDiaria.IdCajaDiaria,
        //            cajaDiaria.Fecha,
        //            cajaDiaria.SaldoInicial,
        //            cajaDiaria.SaldoFinal,
        //            cajaDiaria.Estado
        //        },
        //        MovimientosCaja = movimientosCaja,
        //        TotalVentas = totalVentas,
        //        TotalGastos = totalGastos
        //    };

        //    if (response == null)
        //    {
        //        return NotFound("No se encontraron datos para la fecha especificada.");
        //    }

        //    return Ok(response);
        //}






        //**********************************************************

        [Authorize(Roles = "Administrador")]
        [HttpGet("weekly")]
        public IActionResult WeeklyBoxRe()
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

            decimal totalIngresosSemana = 0;
            decimal totalVentasSemana = 0;
            decimal totalComprasSemana = 0;
            decimal totalSaldoFinalSemana = 0;

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

                totalIngresosSemana += totalVentas;
                totalVentasSemana += totalVentas;
                totalComprasSemana += totalGastos;
                totalSaldoFinalSemana += saldoFinal;

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
                    TotalGastos = totalGastos
                };

                response.Add(cajaResponse);
            }

            var semanaResponse = new
            {
                TotalIngresosSemana = totalIngresosSemana,
                TotalVentasSemana = totalVentasSemana,
                TotalComprasSemana = totalComprasSemana,
                TotalSaldoFinalSemana = totalSaldoFinalSemana,
                CajasSemanales = response
            };

            return Ok(semanaResponse);
        }



        //***************************************************



        //[HttpGet("monthly")]
        //public IActionResult MonthlyBoxRe()
        //{
        //    DateTime currentDate = DateTime.Today;
        //    DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        //    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        //    DateOnly startOfMonthDateOnly = DateOnly.FromDateTime(startOfMonth);
        //    DateOnly endOfMonthDateOnly = DateOnly.FromDateTime(endOfMonth);

        //    //List<CajaDiaria> cajasMensuales = _dbContext.CajaDiaria
        //    //    .Where(c => c.Fecha >= startOfMonthDateOnly && c.Fecha <= endOfMonthDateOnly)
        //    //    .ToList();
        //    List<CajaDiaria> cajasMensuales = _dbContext.CajaDiaria
        //            .Where(c => c.Fecha >= startOfMonthDateOnly && c.Fecha <= endOfMonthDateOnly)
        //            .OrderByDescending(c => c.Fecha) // Ordenar por fecha en orden descendente
        //            .ToList();

        //    if (cajasMensuales.Count == 0)
        //    {
        //        return NotFound("No se encontraron cajas diarias para el mes actual.");
        //    }

        //    decimal totalIngresosMes = 0;
        //    decimal totalVentasMes = 0;
        //    decimal totalComprasMes = 0;
        //    decimal totalSaldoFinalMes = 0;

        //    List<object> response = new List<object>();

        //    foreach (CajaDiaria cajaDiaria in cajasMensuales)
        //    {
        //        var movimientosCaja = (from mc in _dbContext.MovimientoCajas
        //                               where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
        //                               select new
        //                               {
        //                                   mc.IdMovimiento,
        //                                   mc.IdTipoMovimiento,
        //                                   mc.Concepto,
        //                                   mc.Total,
        //                                   Ventas = (from vmc in _dbContext.VentaMovimientoCajas
        //                                             join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
        //                                             where vmc.IdMovimientoCaja == mc.IdMovimiento
        //                                             select new
        //                                             {
        //                                                 v.IdVenta,
        //                                                 v.NumeroComanda,
        //                                                 v.Fecha,
        //                                                 v.Total,
        //                                                 v.IdMesero,
        //                                                 v.IdCliente
        //                                             }).ToList(),
        //                                   Gastos = (from gmc in _dbContext.GastosMovimientoCajas
        //                                             join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
        //                                             where gmc.IdMovimientoCaja == mc.IdMovimiento
        //                                             select new
        //                                             {
        //                                                 g.IdGasto,
        //                                                 g.NumeroDocumento,
        //                                                 g.Fecha,
        //                                                 g.Concepto,
        //                                                 g.Total,
        //                                                 g.IdProveedor
        //                                             }).ToList()
        //                               }).ToList();

        //        decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
        //        decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

        //        decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
        //        cajaDiaria.SaldoFinal = saldoFinal;

        //        totalIngresosMes += totalVentas;
        //        totalVentasMes += totalVentas;
        //        totalComprasMes += totalGastos;
        //        totalSaldoFinalMes += saldoFinal;

        //        var cajaResponse = new
        //        {
        //            CajaDiaria = new
        //            {
        //                cajaDiaria.IdCajaDiaria,
        //                cajaDiaria.Fecha,
        //                cajaDiaria.SaldoInicial,
        //                cajaDiaria.SaldoFinal,
        //                cajaDiaria.Estado
        //            },
        //            MovimientosCaja = movimientosCaja,
        //            TotalVentas = totalVentas,
        //            TotalGastos = totalGastos
        //        };

        //        response.Add(cajaResponse);
        //    }

        //    var mesResponse = new
        //    {
        //        TotalIngresosMes = totalIngresosMes,
        //        TotalVentasMes = totalVentasMes,
        //        TotalComprasMes = totalComprasMes,
        //        TotalSaldoFinalMes = totalSaldoFinalMes,
        //        CajasMensuales = response
        //    };

        //    return Ok(mesResponse);
        //}


        //[HttpGet("mon")]//muestra resultados por semana
        //public IActionResult MonthlyB()
        //{
        //    DateTime currentDate = DateTime.Today;
        //    DateTime startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        //    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        //    DateOnly startOfMonthDateOnly = DateOnly.FromDateTime(startOfMonth);
        //    DateOnly endOfMonthDateOnly = DateOnly.FromDateTime(endOfMonth);

        //    List<CajaDiaria> cajasMensuales = _dbContext.CajaDiaria
        //        .Where(c => c.Fecha >= startOfMonthDateOnly && c.Fecha <= endOfMonthDateOnly)
        //        .OrderByDescending(c => c.Fecha) // Ordenar por fecha en orden descendente
        //        .ToList();

        //    if (cajasMensuales.Count == 0)
        //    {
        //        return NotFound("No se encontraron cajas diarias para el mes actual.");
        //    }

        //    decimal totalIngresosMes = 0;
        //    decimal totalVentasMes = 0;
        //    decimal totalComprasMes = 0;
        //    decimal totalSaldoFinalMes = 0;

        //    List<object> response = new List<object>();

        //    DateTime startOfWeek = startOfMonth;
        //    DateTime endOfWeek = startOfWeek.AddDays(6);

        //    while (startOfWeek <= endOfMonth)
        //    {
        //        DateOnly startOfWeekDateOnly = DateOnly.FromDateTime(startOfWeek);
        //        DateOnly endOfWeekDateOnly = DateOnly.FromDateTime(endOfWeek);

        //        List<CajaDiaria> cajasSemanales = cajasMensuales
        //            .Where(c => c.Fecha >= startOfWeekDateOnly && c.Fecha <= endOfWeekDateOnly)
        //            .ToList();

        //        decimal totalVentasSemana = 0;
        //        decimal totalComprasSemana = 0;
        //        decimal totalSaldoFinalSemana = 0;

        //        List<object> semanaResponse = new List<object>();

        //        foreach (CajaDiaria cajaDiaria in cajasSemanales)
        //        {
        //            var movimientosCaja = (from mc in _dbContext.MovimientoCajas
        //                                   where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
        //                                   select new
        //                                   {
        //                                       mc.IdMovimiento,
        //                                       mc.IdTipoMovimiento,
        //                                       mc.Concepto,
        //                                       mc.Total,
        //                                       Ventas = (from vmc in _dbContext.VentaMovimientoCajas
        //                                                 join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
        //                                                 where vmc.IdMovimientoCaja == mc.IdMovimiento
        //                                                 select new
        //                                                 {
        //                                                     v.IdVenta,
        //                                                     v.NumeroComanda,
        //                                                     v.Fecha,
        //                                                     v.Total,
        //                                                     v.IdMesero,
        //                                                     v.IdCliente
        //                                                 }).ToList(),
        //                                       Gastos = (from gmc in _dbContext.GastosMovimientoCajas
        //                                                 join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
        //                                                 where gmc.IdMovimientoCaja == mc.IdMovimiento
        //                                                 select new
        //                                                 {
        //                                                     g.IdGasto,
        //                                                     g.NumeroDocumento,
        //                                                     g.Fecha,
        //                                                     g.Concepto,
        //                                                     g.Total,
        //                                                     g.IdProveedor
        //                                                 }).ToList()
        //                                   }).ToList();

        //            decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
        //            decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

        //            decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
        //            cajaDiaria.SaldoFinal = saldoFinal;

        //            totalVentasSemana += totalVentas;
        //            totalComprasSemana += totalGastos;
        //            totalSaldoFinalSemana += saldoFinal;

        //            var cajaResponse = new
        //            {
        //                CajaDiaria = new
        //                {
        //                    cajaDiaria.IdCajaDiaria,
        //                    cajaDiaria.Fecha,
        //                    cajaDiaria.SaldoInicial,
        //                    cajaDiaria.SaldoFinal,
        //                    cajaDiaria.Estado
        //                },
        //                MovimientosCaja = movimientosCaja,
        //                TotalVentas = totalVentas,
        //                TotalGastos = totalGastos
        //            };

        //            semanaResponse.Add(cajaResponse);
        //        }

        //        var semanaData = new
        //        {
        //            StartDate = startOfWeek,
        //            EndDate = endOfWeek,
        //            TotalVentasSemana = totalVentasSemana,
        //            TotalComprasSemana = totalComprasSemana,
        //            TotalSaldoFinalSemana = totalSaldoFinalSemana,
        //            CajasSemanales = semanaResponse
        //        };

        //        response.Add(semanaData);

        //        totalVentasMes += totalVentasSemana;
        //        totalComprasMes += totalComprasSemana;
        //        totalSaldoFinalMes += totalSaldoFinalSemana;

        //        startOfWeek = startOfWeek.AddDays(7);
        //        endOfWeek = endOfWeek.AddDays(7);

        //        if (endOfWeek > endOfMonth)
        //        {
        //            endOfWeek = endOfMonth;
        //        }
        //    }

        //    var mesResponse = new
        //    {
        //        TotalIngresosMes = totalVentasMes,
        //        TotalVentasMes = totalVentasMes,
        //        TotalComprasMes = totalComprasMes,
        //        TotalSaldoFinalMes = totalSaldoFinalMes,
        //        Semanas = response
        //    };

        //    return Ok(mesResponse);
        //}


        //[HttpGet("mont")]//por un int funicona
        //public IActionResult MonthlyBj(int month)
        //{
        //    DateTime currentDate = DateTime.Today;
        //    DateTime requestedDate = new DateTime(currentDate.Year, month, 1);

        //    if (requestedDate > currentDate)
        //    {
        //        return BadRequest("El mes solicitado es mayor al mes actual.");
        //    }

        //    DateTime startOfMonth = new DateTime(requestedDate.Year, requestedDate.Month, 1);
        //    DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

        //    DateOnly startOfMonthDateOnly = DateOnly.FromDateTime(startOfMonth);
        //    DateOnly endOfMonthDateOnly = DateOnly.FromDateTime(endOfMonth);

        //    List<CajaDiaria> cajasMensuales = _dbContext.CajaDiaria
        //        .Where(c => c.Fecha >= startOfMonthDateOnly && c.Fecha <= endOfMonthDateOnly)
        //        .OrderByDescending(c => c.Fecha) // Ordenar por fecha en orden descendente
        //        .ToList();

        //    if (cajasMensuales.Count == 0)
        //    {
        //        return NotFound("No se encontraron cajas diarias para el mes solicitado.");
        //    }

        //    decimal totalIngresosMes = 0;
        //    decimal totalVentasMes = 0;
        //    decimal totalComprasMes = 0;
        //    decimal totalSaldoFinalMes = 0;

        //    List<object> response = new List<object>();

        //    DateTime startOfWeek = startOfMonth;
        //    DateTime endOfWeek = startOfWeek.AddDays(6);

        //    while (startOfWeek <= endOfMonth)
        //    {
        //        DateOnly startOfWeekDateOnly = DateOnly.FromDateTime(startOfWeek);
        //        DateOnly endOfWeekDateOnly = DateOnly.FromDateTime(endOfWeek);

        //        List<CajaDiaria> cajasSemanales = cajasMensuales
        //            .Where(c => c.Fecha >= startOfWeekDateOnly && c.Fecha <= endOfWeekDateOnly)
        //            .ToList();

        //        decimal totalVentasSemana = 0;
        //        decimal totalComprasSemana = 0;
        //        decimal totalSaldoFinalSemana = 0;

        //        List<object> semanaResponse = new List<object>();

        //        foreach (CajaDiaria cajaDiaria in cajasSemanales)
        //        {
        //            var movimientosCaja = (from mc in _dbContext.MovimientoCajas
        //                                   where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
        //                                   select new
        //                                   {
        //                                       mc.IdMovimiento,
        //                                       mc.IdTipoMovimiento,
        //                                       mc.Concepto,
        //                                       mc.Total,
        //                                       Ventas = (from vmc in _dbContext.VentaMovimientoCajas
        //                                                 join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
        //                                                 where vmc.IdMovimientoCaja == mc.IdMovimiento
        //                                                 select new
        //                                                 {
        //                                                     v.IdVenta,
        //                                                     v.NumeroComanda,
        //                                                     v.Fecha,
        //                                                     v.Total,
        //                                                     v.IdMesero,
        //                                                     v.IdCliente
        //                                                 }).ToList(),
        //                                       Gastos = (from gmc in _dbContext.GastosMovimientoCajas
        //                                                 join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
        //                                                 where gmc.IdMovimientoCaja == mc.IdMovimiento
        //                                                 select new
        //                                                 {
        //                                                     g.IdGasto,
        //                                                     g.NumeroDocumento,
        //                                                     g.Fecha,
        //                                                     g.Concepto,
        //                                                     g.Total,
        //                                                     g.IdProveedor
        //                                                 }).ToList()
        //                                   }).ToList();

        //            decimal totalVentas = movimientosCaja.SelectMany(m => m.Ventas).Sum(v => v.Total) ?? 0;
        //            decimal totalGastos = movimientosCaja.SelectMany(m => m.Gastos).Sum(g => g.Total) ?? 0;

        //            decimal saldoFinal = ((cajaDiaria.SaldoInicial + totalVentas) - totalGastos) ?? 0;
        //            cajaDiaria.SaldoFinal = saldoFinal;

        //            totalVentasSemana += totalVentas;
        //            totalComprasSemana += totalGastos;
        //            totalSaldoFinalSemana += saldoFinal;

        //            var cajaResponse = new
        //            {
        //                CajaDiaria = new
        //                {
        //                    cajaDiaria.IdCajaDiaria,
        //                    cajaDiaria.Fecha,
        //                    cajaDiaria.SaldoInicial,
        //                    cajaDiaria.SaldoFinal,
        //                    cajaDiaria.Estado
        //                },
        //                MovimientosCaja = movimientosCaja,
        //                TotalVentas = totalVentas,
        //                TotalGastos = totalGastos
        //            };

        //            semanaResponse.Add(cajaResponse);
        //        }

        //        var semanaData = new
        //        {
        //            StartDate = startOfWeek,
        //            EndDate = endOfWeek,
        //            TotalVentasSemana = totalVentasSemana,
        //            TotalComprasSemana = totalComprasSemana,
        //            TotalSaldoFinalSemana = totalSaldoFinalSemana,
        //            CajasSemanales = semanaResponse
        //        };

        //        response.Add(semanaData);

        //        totalVentasMes += totalVentasSemana;
        //        totalComprasMes += totalComprasSemana;
        //        totalSaldoFinalMes += totalSaldoFinalSemana;

        //        startOfWeek = startOfWeek.AddDays(7);
        //        endOfWeek = endOfWeek.AddDays(7);

        //        if (endOfWeek > endOfMonth)
        //        {
        //            endOfWeek = endOfMonth;
        //        }
        //    }

        //    var mesResponse = new
        //    {
        //        TotalIngresosMes = totalVentasMes,
        //        TotalVentasMes = totalVentasMes,
        //        TotalComprasMes = totalComprasMes,
        //        TotalSaldoFinalMes = totalSaldoFinalMes,
        //        Semanas = response
        //    };

        //    return Ok(mesResponse);
        //}

        [Authorize(Roles = "Administrador")]
        [HttpGet("monts")]//a usar
        public IActionResult MonthlyBdj(string month)
        {
            DateTime currentDate = DateTime.Today;
            DateTime requestedDate;

            try
            {
                requestedDate = DateTime.ParseExact(month, "MMMM", CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
                return BadRequest("El mes ingresado no es válido.");
            }

            if (requestedDate > currentDate)
            {
                return BadRequest("El mes solicitado es mayor al mes actual.");
            }

            DateTime startOfMonth = new DateTime(requestedDate.Year, requestedDate.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            DateOnly startOfMonthDateOnly = DateOnly.FromDateTime(startOfMonth);
            DateOnly endOfMonthDateOnly = DateOnly.FromDateTime(endOfMonth);

            List<CajaDiaria> cajasMensuales = _dbContext.CajaDiaria
                .Where(c => c.Fecha >= startOfMonthDateOnly && c.Fecha <= endOfMonthDateOnly)
                .OrderByDescending(c => c.Fecha) // Ordenar por fecha en orden descendente
                .ToList();

            if (cajasMensuales.Count == 0)
            {
                return NotFound("No se encontraron cajas diarias para el mes solicitado.");
            }

            decimal totalIngresosMes = 0;
            decimal totalVentasMes = 0;
            decimal totalComprasMes = 0;
            decimal totalSaldoFinalMes = 0;

            List<object> response = new List<object>();

            DateTime startOfWeek = startOfMonth;
            DateTime endOfWeek = startOfWeek.AddDays(6);

            while (startOfWeek <= endOfMonth)
            {
                DateOnly startOfWeekDateOnly = DateOnly.FromDateTime(startOfWeek);
                DateOnly endOfWeekDateOnly = DateOnly.FromDateTime(endOfWeek);

                List<CajaDiaria> cajasSemanales = cajasMensuales
                    .Where(c => c.Fecha >= startOfWeekDateOnly && c.Fecha <= endOfWeekDateOnly)
                    .ToList();

                decimal totalVentasSemana = 0;
                decimal totalComprasSemana = 0;
                decimal totalSaldoFinalSemana = 0;

                List<object> semanaResponse = new List<object>();

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

                    totalVentasSemana += totalVentas;
                    totalComprasSemana += totalGastos;
                    totalSaldoFinalSemana += saldoFinal;

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
                        TotalGastos = totalGastos
                    };

                    semanaResponse.Add(cajaResponse);
                }

                var semanaData = new
                {
                    StartDate = startOfWeek.ToString("dddd, MMMM d, yyyy"),
                    EndDate = endOfWeek.ToString("dddd, MMMM d, yyyy"),
                    TotalVentasSemana = totalVentasSemana,
                    TotalComprasSemana = totalComprasSemana,
                    TotalSaldoFinalSemana = totalSaldoFinalSemana,
                    CajasSemanales = semanaResponse
                };

                response.Add(semanaData);

                totalVentasMes += totalVentasSemana;
                totalComprasMes += totalComprasSemana;
                totalSaldoFinalMes += totalSaldoFinalSemana;

                startOfWeek = startOfWeek.AddDays(7);
                endOfWeek = endOfWeek.AddDays(7);

                if (endOfWeek > endOfMonth)
                {
                    endOfWeek = endOfMonth;
                }
            }

            var mesResponse = new
            {
                Title = requestedDate.ToString("MMMM", CultureInfo.CurrentCulture),
                TotalIngresosMes = totalIngresosMes,
                TotalVentasMes = totalVentasMes,
                TotalComprasMes = totalComprasMes,
                TotalSaldoFinalMes = totalSaldoFinalMes,
                Semanas = response
            };
            if (mesResponse == null)
            {
                return NotFound("No se encontraron datos para la fecha especificada.");
            }

            return Ok(mesResponse);
        }


    }
}
