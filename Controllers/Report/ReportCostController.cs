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
using restaurante_web_app.Data.DTOs;
using System.Text.Json.Serialization;

namespace restaurante_web_app.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportCostController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public ReportCostController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("day")]
        public IActionResult GenerateReporGas(DateTime fecha)
        {


            //DateTime today = DateTime.Today;
            DateOnly selectedDate = DateOnly.FromDateTime(fecha);
            DateOnly today = DateOnly.FromDateTime(DateTime.Today); //PARA MOSTRAR HOY
            //DateOnly selectedDate = DateOnly.FromDateTime(fecha);
            //DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3));


            var data = (from p in _dbContext.Proveedores
                        join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                        //join d in _dbContext.DetalleVenta on v.IdVenta equals d.IdVenta
                        //join m in _dbContext.Menus on d.IdPlatillo equals m.IdPlatillo
                        where g.Fecha == selectedDate
                        group new { g, p } by new { g.IdGasto, g.NumeroDocumento, p.IdProveedor, p.Nombre, p.Telefono } into groupedData
                        select new
                        {
                            Proveedor = new
                            {
                                groupedData.Key.IdProveedor,
                                groupedData.Key.Nombre,
                                groupedData.Key.Telefono,
                                groupedData.Key.NumeroDocumento
                            },
                            Total = groupedData.Sum(item => item.g.Total),
                            Gastos = groupedData.Select(item => new
                            {
                                item.g.IdGasto,
                                //item.g.NumeroDocumento,
                                item.g.Fecha,
                                item.g.Concepto,
                                item.g.IdProveedor,
                                //item.p.Nombre,
                                //item.p.Telefono
                            })
                        }).ToList();
            // Validar la fecha o cualquier otro criterio necesario



            if (data == null)
            {
                return NotFound("No se encontraron datos para la fecha especificada.");
            }

            return Ok(data);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("week")]
        public IActionResult GeneratePdfWeek(int month, int weekNumber)
        {
            if (month > 12 || weekNumber > 31)
            {
                return BadRequest("No se puede realizar la acción");
            }

            int currentYear = DateTime.Today.Year;

            // Calcular la fecha de inicio de la semana y la fecha de fin de la semana
            DateTime weekStart = GetFirstDayOfWeek(currentYear, month, weekNumber);
            DateTime weekEnd = weekStart.AddDays(7);

            // Convert to DateOnly
            DateOnly weekStartDate = DateOnly.FromDateTime(weekStart);
            DateOnly weekEndDate = DateOnly.FromDateTime(weekEnd);

            // Get the data from the database for the current week
            var data = (from p in _dbContext.Proveedores
                        join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                        where g.Fecha >= weekStartDate && g.Fecha < weekEndDate
                        group new { g, p } by new { g.IdGasto, g.NumeroDocumento, p.IdProveedor, p.Nombre, p.Telefono } into groupedData
                        select new
                        {
                            Proveedor = new
                            {
                                groupedData.Key.IdProveedor,
                                groupedData.Key.Nombre,
                                groupedData.Key.Telefono,
                                groupedData.Key.NumeroDocumento
                            },
                            Total = groupedData.Sum(item => item.g.Total),
                            Gastos = groupedData.Select(item => new
                            {
                                item.g.IdGasto,
                                //item.g.NumeroDocumento,
                                item.g.Fecha,
                                item.g.Concepto,
                                item.g.IdProveedor,
                                //item.p.Nombre,
                                //item.p.Telefono
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


        [Authorize(Roles = "Administrador")]
        [HttpGet("month")]
        public IActionResult GenerateYear(string month)
        {

            int currentYear = DateTime.Today.Year;

            // Obtener el número del mes a partir del nombre ingresado por el usuario
            int monthNumber = GetMonthNumber(month);

            // Verificar si se encontró un número de mes válido
            if (monthNumber == 0 || monthNumber > 12)
            {
                return BadRequest("Mes inválido");
            }

            // Obtener el primer día del mes seleccionado
            DateTime monthStart = new DateTime(currentYear, monthNumber, 1);

            // Obtener el primer día del mes siguiente
            DateTime nextMonthStart = monthStart.AddMonths(1);

            // Convertir a DateOnly
            DateOnly monthStartDate = DateOnly.FromDateTime(monthStart);
            DateOnly nextMonthStartDate = DateOnly.FromDateTime(nextMonthStart);

            // Obtener los datos de la base de datos para el mes actual
            var data = (from p in _dbContext.Proveedores
                        join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                        //where g.Fecha >= weekStartDate && g.Fecha < weekEndDate
                        where g.Fecha >= monthStartDate && g.Fecha < nextMonthStartDate
                        group new { g, p } by new { g.IdGasto, g.NumeroDocumento, p.IdProveedor, p.Nombre, p.Telefono } into groupedData
                        select new
                        {
                            Proveedor = new
                            {
                                groupedData.Key.IdProveedor,
                                groupedData.Key.Nombre,
                                groupedData.Key.Telefono,
                                groupedData.Key.NumeroDocumento
                            },
                            Total = groupedData.Sum(item => item.g.Total),
                            Gastos = groupedData.Select(item => new
                            {
                                item.g.IdGasto,
                                //item.g.NumeroDocumento,
                                item.g.Fecha,
                                item.g.Concepto,
                                item.g.IdProveedor,
                                //item.p.Nombre,
                                //item.p.Telefono
                            })
                        }).ToList();

            if (data == null)
            {
                return NotFound("No se encontraron datos para la fecha especificada.");
            }

            return Ok(data);
        }

        private int GetMonthNumber(string month)
        {
            DateTimeFormatInfo dateTimeFormat = new DateTimeFormatInfo();
            List<string> monthNames = dateTimeFormat.MonthNames.ToList();
            int monthNumber = monthNames.FindIndex(m => m.Equals(month, StringComparison.OrdinalIgnoreCase)) + 1;
            return monthNumber;
        }



        [Authorize(Roles = "Administrador")]
        [HttpGet("rango")]
        public IActionResult GenerateReporDayRange(DateTime fechaDesde, DateTime fechaHasta)
        {
            DateOnly selectedDateDesde = DateOnly.FromDateTime(fechaDesde);
            DateOnly selectedDateHasta = DateOnly.FromDateTime(fechaHasta);

            var data = (from p in _dbContext.Proveedores
                        join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                        //where g.Fecha >= weekStartDate && g.Fecha < weekEndDate
                        //where g.Fecha >= monthStartDate && g.Fecha < nextMonthStartDate
                        where g.Fecha >= selectedDateDesde && g.Fecha <= selectedDateHasta
                        group new { g, p } by new { g.IdGasto, g.NumeroDocumento, p.IdProveedor, p.Nombre, p.Telefono } into groupedData
                        select new
                        {
                            Proveedor = new
                            {
                                groupedData.Key.IdProveedor,
                                groupedData.Key.Nombre,
                                groupedData.Key.Telefono,
                                groupedData.Key.NumeroDocumento
                            },
                            Total = groupedData.Sum(item => item.g.Total),
                            Gastos = groupedData.Select(item => new
                            {
                                item.g.IdGasto,
                                //item.g.NumeroDocumento,
                                item.g.Fecha,
                                item.g.Concepto,
                                item.g.IdProveedor,
                                //item.p.Nombre,
                                //item.p.Telefono
                            })
                        }).ToList();


            if (data == null)
            {
                return NotFound("No se encontraron datos para la fecha especificada.");
            }

            return Ok(data);

        }



        [Authorize(Roles = "Administrador")]
        [HttpGet("year")]
        public IActionResult GenerateYear()
        {
            int currentYear = DateTime.Today.Year;

            // Obtener la fecha de inicio y fin del año actual
            DateTime yearStart = new DateTime(currentYear, 1, 1);
            DateTime yearEnd = yearStart.AddYears(1);

            // Convertir a DateOnly
            DateOnly yearStartDate = DateOnly.FromDateTime(yearStart);
            DateOnly yearEndDate = DateOnly.FromDateTime(yearEnd);

            // Obtener los datos de la base de datos para el año actual
            var data = (from p in _dbContext.Proveedores
                        join g in _dbContext.Gastos on p.IdProveedor equals g.IdProveedor
                        where g.Fecha >= yearStartDate && g.Fecha < yearEndDate
                        group new { g, p } by new { g.IdGasto, g.NumeroDocumento, p.IdProveedor, p.Nombre, p.Telefono } into groupedData
                        select new
                        {
                            Proveedor = new
                            {
                                groupedData.Key.IdProveedor,
                                groupedData.Key.Nombre,
                                groupedData.Key.Telefono,
                                groupedData.Key.NumeroDocumento
                            },
                            Total = groupedData.Sum(item => item.g.Total),
                            Gastos = groupedData.Select(item => new
                            {
                                item.g.IdGasto,
                                //item.g.NumeroDocumento,
                                item.g.Fecha,
                                item.g.Concepto,
                                item.g.IdProveedor,
                                //item.p.Nombre,
                                //item.p.Telefono
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
