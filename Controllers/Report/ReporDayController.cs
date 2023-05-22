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

namespace restaurante_web_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportDayController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public ReportDayController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("day")]
        //public IActionResult GenerateReporDay(DateTime fecha)
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
                        group new { v, d, m } by new { c.IdCliente, c.NombreApellido, c.Institucion } into groupedData
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
                        group new { v, d, m } by new { c.IdCliente, c.NombreApellido, c.Institucion } into groupedData
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
                return NotFound("No se encontraron datos para el rango de fechas especificado.");
            }

            return Ok(data);
        }

    }
}