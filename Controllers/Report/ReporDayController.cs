using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Models;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

using System;
using System.IO;
using System.Linq;
using iTextSharp.text.pdf.draw;
using System.Drawing;
using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace restaurante_web_app.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportDayController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public ReportDayController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("day")]
        public IActionResult GenerateReporDay(DateTime fecha)
        {


            //DateTime today = DateTime.Today;
            DateOnly selectedDate = DateOnly.FromDateTime(fecha);
            DateOnly today = DateOnly.FromDateTime(DateTime.Today); //PARA MOSTRAR HOY
            //DateOnly selectedDate = DateOnly.FromDateTime(fecha);
            //DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3));


            var data = (from c in _dbContext.Clientes
                        join v in _dbContext.Ventas on c.IdCliente equals v.IdCliente
                        join d in _dbContext.DetalleVenta on v.IdVenta equals d.IdVenta
                        join m in _dbContext.Menus on d.IdPlatillo equals m.IdPlatillo
                        where v.Fecha == selectedDate
                        group new { v, d, m } by new { v.IdVenta, c.IdCliente, c.NombreApellido, c.Institucion } into groupedData
                        select new
                        {
                            Cliente = new
                            {
                                groupedData.Key.IdCliente,
                                groupedData.Key.NombreApellido,
                                groupedData.Key.Institucion
                            },
                            Total = groupedData.Sum(item => item.d.Subtotal),
                            Ventas = groupedData.Select(item => new
                            {
                                item.v.IdVenta,
                                item.v.NumeroComanda,
                                item.v.Fecha,
                                item.v.Total,
                                item.v.IdMesero,
                                item.d.Cantidad,
                                item.d.Subtotal,
                                item.m.Platillo,
                                item.m.Precio
                            })
                        }).ToList();
            // Validar la fecha o cualquier otro criterio necesario
            

            
            if (data == null)
            {
                return NotFound("No se encontraron datos para la fecha especificada.");
            }

            //if (data.Count > 0)
            //{
            //    return Ok(data);
            //}
            //else
            //{
            //    return NotFound("No se encontraron ventas para hoy.");
            //}

            return Ok(data);
        }


        [HttpGet("reportweek")]
        public IActionResult GeneratePdfWeek(int month, int weekNumber)
        {
            int currentYear = DateTime.Today.Year;

            // Calcular la fecha de inicio de la semana y la fecha de fin de la semana
            DateTime weekStart = GetFirstDayOfWeek(currentYear, month, weekNumber);
            DateTime weekEnd = weekStart.AddDays(7);

            // Convert to DateOnly
            DateOnly weekStartDate = DateOnly.FromDateTime(weekStart);
            DateOnly weekEndDate = DateOnly.FromDateTime(weekEnd);

            // Get the data from the database for the current week
            var data = (from c in _dbContext.Clientes
                       join v in _dbContext.Ventas on c.IdCliente equals v.IdCliente
                       join d in _dbContext.DetalleVenta on v.IdVenta equals d.IdVenta
                       join m in _dbContext.Menus on d.IdPlatillo equals m.IdPlatillo
                       where v.Fecha >= weekStartDate && v.Fecha < weekEndDate
                       group new { v, d, m } by new { v.IdVenta, c.IdCliente, c.NombreApellido, c.Institucion } into groupedData
                       select new
                       {
                           Cliente = new
                           {
                               groupedData.Key.IdCliente,
                               groupedData.Key.NombreApellido,
                               groupedData.Key.Institucion
                           },
                           Total = groupedData.Sum(item => item.d.Subtotal),
                           Ventas = groupedData.Select(item => new
                           {
                               item.v.IdVenta,
                               item.v.NumeroComanda,
                               item.v.Fecha,
                               item.v.Total,
                               item.v.IdMesero,
                               item.d.Cantidad,
                               item.d.Subtotal,
                               item.m.Platillo,
                               item.m.Precio
                           })
                       }).ToList();
            if (data == null)
            {
                return NotFound("No se encontraron datos para la fecha especificada.");
            }


            return Ok(data);

        }

        private DateTime GetFirstDayOfWeek(int year, int month, int weekNumber)
        {
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DayOfWeek dayOfWeek = firstDayOfMonth.DayOfWeek;
            int daysToAdd = (int)DayOfWeek.Monday - (int)dayOfWeek;
            if (daysToAdd > 0)
                daysToAdd -= 7;

            int weeksToAdd = weekNumber - 1;

            return firstDayOfMonth.AddDays(daysToAdd + (7 * weeksToAdd));
        }


        [HttpGet("month")]
        public IActionResult GenerateYear(int month)
        {
            int currentYear = DateTime.Today.Year;

            // Obtener el primer día del mes seleccionado
            DateTime monthStart = new DateTime(currentYear, month, 1);

            // Obtener el primer día del mes siguiente
            DateTime nextMonthStart = monthStart.AddMonths(1);

            // Convertir a DateOnly
            DateOnly monthStartDate = DateOnly.FromDateTime(monthStart);
            DateOnly nextMonthStartDate = DateOnly.FromDateTime(nextMonthStart);

            // Obtener los datos de la base de datos para el mes actual
            var data = (from c in _dbContext.Clientes
                        join v in _dbContext.Ventas on c.IdCliente equals v.IdCliente
                        join d in _dbContext.DetalleVenta on v.IdVenta equals d.IdVenta
                        join m in _dbContext.Menus on d.IdPlatillo equals m.IdPlatillo
                        where v.Fecha >= monthStartDate && v.Fecha < nextMonthStartDate
                        group new { v, d, m } by new { v.IdVenta, c.IdCliente, c.NombreApellido, c.Institucion } into groupedData
                        select new
                        {
                            Cliente = new
                            {
                                groupedData.Key.IdCliente,
                                groupedData.Key.NombreApellido,
                                groupedData.Key.Institucion
                            },
                            Total = groupedData.Sum(item => item.d.Subtotal),
                            Ventas = groupedData.Select(item => new
                            {
                                item.v.IdVenta,
                                item.v.NumeroComanda,
                                item.v.Fecha,
                                item.v.Total,
                                item.v.IdMesero,
                                item.d.Cantidad,
                                item.d.Subtotal,
                                item.m.Platillo,
                                item.m.Precio
                            })
                        }).ToList();

            if (data == null || data.Count == 0)
            {
                return NotFound("No se encontraron datos para el mes especificado.");
            }

            return Ok(data);
        }





        [Authorize(Roles = "Administrador")]
        [HttpGet("rango")]
        public IActionResult GenerateReporDayRange(DateTime fechaDesde, DateTime fechaHasta)
        {
            DateOnly selectedDateDesde = DateOnly.FromDateTime(fechaDesde);
            DateOnly selectedDateHasta = DateOnly.FromDateTime(fechaHasta);

            var data = (from c in _dbContext.Clientes
                        join v in _dbContext.Ventas on c.IdCliente equals v.IdCliente
                        join d in _dbContext.DetalleVenta on v.IdVenta equals d.IdVenta
                        join m in _dbContext.Menus on d.IdPlatillo equals m.IdPlatillo
                        where v.Fecha >= selectedDateDesde && v.Fecha <= selectedDateHasta
                        group new { v, d, m } by new { v.IdVenta, c.IdCliente, c.NombreApellido, c.Institucion } into groupedData
                        select new
                        {
                            Cliente = new
                            {
                                groupedData.Key.IdCliente,
                                groupedData.Key.NombreApellido,
                                groupedData.Key.Institucion
                            },
                            Total = groupedData.Sum(item => item.d.Subtotal),
                            Ventas = groupedData.Select(item => new
                            {
                                item.v.IdVenta,
                                item.v.NumeroComanda,
                                item.v.Fecha,
                                item.v.Total,
                                item.v.IdMesero,
                                item.d.Cantidad,
                                item.d.Subtotal,
                                item.m.Platillo,
                                item.m.Precio
                            })
                        }).ToList();

            
            if (data == null)
            {
                return NotFound("No se encontraron datos para la fecha especificada.");
            }

            return Ok(data);

        }

    }
}