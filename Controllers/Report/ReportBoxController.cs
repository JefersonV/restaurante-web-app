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




        //[HttpGet("{fecha}")]

        //public ActionResult GetDailyReport()
        //{
        //    // Obtener la fecha de hoy
        //    DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Now);

        //    // Obtener la caja diaria correspondiente a la fecha de hoy
        //    var cajaDiaria = _dbContext.CajaDiaria
        //        .Include(c => c.MovimientoCajas)
        //            .ThenInclude(m => m.IdTipoMovimientoNavigation)
        //        .Include(c => c.MovimientoCajas)
        //            .ThenInclude(m => m.GastosMovimientoCajas)
        //                .ThenInclude(gmc => gmc.IdGastoNavigation)
        //                    .ThenInclude(g => g.IdProveedorNavigation)
        //        .Include(c => c.MovimientoCajas)
        //            .ThenInclude(m => m.VentaMovimientoCajas)
        //                .ThenInclude(vmc => vmc.IdVentaNavigation)
        //                    .ThenInclude(v => v.DetalleVenta)
        //        .FirstOrDefault(c => c.Fecha == fechaHoy);

        //    if (cajaDiaria == null)
        //    {
        //        return NotFound();
        //    }

        //    // Construir el reporte de caja diario
        //    var reporte = new
        //    {
        //        CajaDiaria = new
        //        {
        //            cajaDiaria.IdCajaDiaria,
        //            cajaDiaria.Fecha,
        //            cajaDiaria.SaldoInicial,
        //            cajaDiaria.SaldoFinal,
        //            cajaDiaria.Estado
        //        },
        //        MovimientosCaja = cajaDiaria.MovimientoCajas.Select(m => new
        //        {
        //            m.IdMovimiento,
        //            //TipoMovimiento = m.IdTipoMovimientoNavigation,
        //            //m.Concepto,
        //            //m.Total,
        //            Gastos = m.GastosMovimientoCajas.Select(gmc => new
        //            {
        //                gmc.IdGastoNavigation.IdGasto,
        //                gmc.IdGastoNavigation.NumeroDocumento,
        //                gmc.IdGastoNavigation.Fecha,
        //                gmc.IdGastoNavigation.Concepto,
        //                gmc.IdGastoNavigation.Total,
        //                Proveedor = gmc.IdGastoNavigation.IdProveedorNavigation
        //            }),
        //            Ventas = m.VentaMovimientoCajas.Select(vmc => new
        //            {
        //                vmc.IdVentaNavigation.IdVenta,
        //                vmc.IdVentaNavigation.NumeroComanda,
        //                vmc.IdVentaNavigation.Fecha,
        //                vmc.IdVentaNavigation.Total,
        //                vmc.IdVentaNavigation.IdMesero,
        //                vmc.IdVentaNavigation.IdCliente,
        //                //DetalleVenta = vmc.IdVentaNavigation.DetalleVenta.Select(dv => new
        //                //{
        //                //    dv.IdPlatillo,
        //                //    dv.IdPlatilloNavigation.Platillo,
        //                //    dv.IdPlatilloNavigation.Precio
        //                //})
        //            })
        //        })
        //    };

        //    return Ok(reporte);
        //}



        //public async Task<ActionResult> GetDailyReport()
        //{
        //    // Obtener la fecha de hoy
        //    DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Now);

        //    // Obtener la caja diaria correspondiente a la fecha de hoy
        //    var cajaDiaria = await _dbContext.CajaDiaria
        //    .Include(c => c.MovimientoCajas)
        //        .ThenInclude(m => m.IdTipoMovimientoNavigation)
        //    .Include(c => c.MovimientoCajas)
        //        .ThenInclude(m => m.GastosMovimientoCajas)
        //            .ThenInclude(gmc => gmc.IdGastoNavigation)
        //                .ThenInclude(g => g.IdProveedorNavigation)
        //    .Include(c => c.MovimientoCajas)
        //        .ThenInclude(m => m.VentaMovimientoCajas)
        //            .ThenInclude(vmc => vmc.IdVentaNavigation)
        //                .ThenInclude(v => v.DetalleVenta)
        //    .FirstOrDefaultAsync(c => c.Fecha == fechaHoy);

        //    if (cajaDiaria == null)
        //    {
        //        return NotFound();
        //    }

        //    // Construir el reporte de caja diario
        //    var reporte = new
        //    {
        //        CajaDiaria = new
        //        {
        //            cajaDiaria.IdCajaDiaria,
        //            cajaDiaria.Fecha,
        //            cajaDiaria.SaldoInicial,
        //            cajaDiaria.SaldoFinal,
        //            cajaDiaria.Estado
        //        },
        //        MovimientosCaja = cajaDiaria.Select(m => new
        //        {
        //            m.IdMovimiento,
        //            TipoMovimiento = m.IdTipoMovimiento,
        //            m.Concepto,
        //            m.Total,
        //            Gastos = m.Gastos.Select(g => new
        //            {
        //                g.IdGasto,
        //                g.NumeroDocumento,
        //                g.Fecha,
        //                g.Concepto,
        //                g.Total,
        //                Proveedor = g.IdProveedor
        //            }),
        //            Ventas = m.Ventas.Select(vmc => new
        //            {
        //                vmc.IdVenta,
        //                vmc.NumeroComanda,
        //                vmc.Fecha,
        //                vmc.Total,
        //                vmc.IdMesero,
        //                vmc.IdCliente,
        //                DetalleVenta = vmc.Detalles.Select(dv => new
        //                {
        //                    dv.IdPlatillo,
        //                    dv.Platillo,
        //                    dv.Precio
        //                })
        //            })
        //        })
        //    };

        //    return Ok(reporte);
        //}


        //[HttpGet]
        //public async Task<ActionResult> GetDailyReport()
        //{
        //    // Obtener la fecha de hoy
        //    DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Now);

        //    // Obtener la caja diaria correspondiente a la fecha de hoy con sus relaciones cargadas
        //    var cajaDiaria = await _dbContext.CajaDiaria
        //        .Include(c => c.MovimientoCajas)
        //            .ThenInclude(m => m.IdTipoMovimientoNavigation)
        //        .Include(c => c.MovimientoCajas)
        //            .ThenInclude(m => m.GastosMovimientoCajas)
        //                .ThenInclude(gmc => gmc.IdGastoNavigation)
        //                    .ThenInclude(g => g.IdProveedorNavigation)
        //        .Include(c => c.MovimientoCajas)
        //            .ThenInclude(m => m.VentaMovimientoCajas)
        //                .ThenInclude(vmc => vmc.IdVentaNavigation)
        //        .FirstOrDefaultAsync(c => c.Fecha == fechaHoy);

        //    if (cajaDiaria == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(cajaDiaria);
        //}

        //[Authorize(Roles = "Administrador")]
        [HttpGet("daily")]
        public async Task<ActionResult> GetDailyReport()
        {
            // Obtener la fecha de hoy
            DateOnly fechaHoy = DateOnly.FromDateTime(DateTime.Now);

            // Obtener la caja diaria correspondiente a la fecha de hoy con sus relaciones cargadas
            var cajaDiaria = await _dbContext.CajaDiaria
                .Include(c => c.MovimientoCajas)
                    .ThenInclude(m => m.IdTipoMovimientoNavigation)
                .Include(c => c.MovimientoCajas)
                    .ThenInclude(m => m.GastosMovimientoCajas)
                        .ThenInclude(gmc => gmc.IdGastoNavigation)
                            .ThenInclude(g => g.IdProveedorNavigation)
                .Include(c => c.MovimientoCajas)
                    .ThenInclude(m => m.VentaMovimientoCajas)
                        .ThenInclude(vmc => vmc.IdVentaNavigation)
                            .ThenInclude(v => v.DetalleVenta)
                                .ThenInclude(dv => dv.IdPlatilloNavigation)
                .FirstOrDefaultAsync(c => c.Fecha == fechaHoy);

            if (cajaDiaria == null)
            {
                return NotFound();
            }

            // Mapear la entidad a DTO
            var cajaDiariaDTO = new CajaDiariaDTO
            {
                IdCajaDiaria = cajaDiaria.IdCajaDiaria,
                Fecha = cajaDiaria.Fecha,
                SaldoInicial = cajaDiaria.SaldoInicial,
                SaldoFinal = cajaDiaria.SaldoFinal,
                Estado = cajaDiaria.Estado,
                MovimientosCaja = cajaDiaria.MovimientoCajas.Select(m => new MovimientoCajaDTO
                {
                    IdMovimiento = m.IdMovimiento,
                    TipoMovimiento = new TipoMovimientoCajaDTO
                    {
                        IdTipoMovimiento = m.IdTipoMovimientoNavigation.IdTipoMovimiento,
                        //Tipo = m.IdTipoMovimientoNavigation.Tipo
                    },
                    //Concepto = m.Concepto,
                    //Total = m.Total,
                    Gastos = m.GastosMovimientoCajas.Select(gmc => new GastoDTO
                    {
                        IdGasto = gmc.IdGastoNavigation.IdGasto,
                        NumeroDocumento = gmc.IdGastoNavigation.NumeroDocumento,
                        Fecha = gmc.IdGastoNavigation.Fecha,
                        Concepto = gmc.IdGastoNavigation.Concepto,
                        Total = gmc.IdGastoNavigation.Total,
                        Proveedor = new ProveedorDTO
                        {
                            IdProveedor = gmc.IdGastoNavigation.IdProveedorNavigation.IdProveedor,
                            Nombre = gmc.IdGastoNavigation.IdProveedorNavigation.Nombre,
                            //Telefono = gmc.IdGastoNavigation.IdProveedorNavigation.Telefono
                        }
                    }).ToList(),
                    Ventas = m.VentaMovimientoCajas.Select(vmc => new VentaDTO
                    {
                        IdVenta = vmc.IdVentaNavigation.IdVenta,
                        NumeroComanda = vmc.IdVentaNavigation.NumeroComanda,
                        Fecha = vmc.IdVentaNavigation.Fecha,
                        Total = vmc.IdVentaNavigation.Total,
                        IdMesero = vmc.IdVentaNavigation.IdMesero,
                        IdCliente = vmc.IdVentaNavigation.IdCliente,
                        DetalleVenta = vmc.IdVentaNavigation.DetalleVenta.Select(dv => new DetalleVentaDTO
                        {
                            IdPlatillo = dv.IdPlatilloNavigation.IdPlatillo,
                            Platillo = dv.IdPlatilloNavigation.Platillo,
                            Precio = dv.IdPlatilloNavigation.Precio
                        }).ToList()
                    }).ToList()
                }).ToList()
            };

            return Ok(cajaDiariaDTO);
        }



        //[HttpGet]

        //public async Task<ActionResult> GetDailyReport()
        //{
        //    // Obtener la fecha actual
        //    //var fechaActual = DateTime.Today;
        //    DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Today);

        //    // Obtener la caja diaria correspondiente a la fecha actual

        //    var cajaDiaria = await _dbContext.CajaDiaria
        //        .Include(cd => cd.MovimientoCajas)
        //            .ThenInclude(mc => mc.IdTipoMovimientoNavigation)
        //        .Include(cd => cd.MovimientoCajas)
        //            .ThenInclude(mc => mc.GastosMovimientoCajas)
        //                .ThenInclude(gmc => gmc.IdGastoNavigation)
        //                    .ThenInclude(g => g.IdProveedorNavigation)
        //        .Include(cd => cd.MovimientoCajas)
        //            .ThenInclude(mc => mc.VentaMovimientoCajas)
        //                .ThenInclude(vmc => vmc.IdVentaNavigation)
        //                    .ThenInclude(v => v.DetalleVenta)
        //                        .ThenInclude(dv => dv.IdPlatilloNavigation)
        //        .FirstOrDefaultAsync(cd => cd.Fecha == fechaActual);

        //    if (cajaDiaria == null)
        //    {
        //        return NotFound();
        //    }

        //    // Construir el reporte de caja diario
        //    var reporte = new
        //    {
        //        CajaDiaria = new
        //        {
        //            cajaDiaria.IdCajaDiaria,
        //            cajaDiaria.Fecha,
        //            cajaDiaria.SaldoInicial,
        //            cajaDiaria.SaldoFinal,
        //            cajaDiaria.Estado
        //        },
        //        MovimientosCaja = cajaDiaria.MovimientoCajas.Select(m => new
        //        {
        //            m.IdMovimiento,
        //            //TipoMovimiento = m.IdTipoMovimientoNavigation,
        //            //m.Concepto,
        //            //m.Total,
        //            Gastos = m.GastosMovimientoCajas.Select(gmc => new
        //            {
        //                gmc.IdGastoNavigation.IdGasto,
        //                gmc.IdGastoNavigation.NumeroDocumento,
        //                gmc.IdGastoNavigation.Fecha,
        //                gmc.IdGastoNavigation.Concepto,
        //                gmc.IdGastoNavigation.Total,
        //                Proveedor = gmc.IdGastoNavigation.IdProveedorNavigation
        //            }),
        //            Ventas = m.VentaMovimientoCajas.Select(vmc => new
        //            {
        //                vmc.IdVentaNavigation.IdVenta,
        //                vmc.IdVentaNavigation.NumeroComanda,
        //                vmc.IdVentaNavigation.Fecha,
        //                vmc.IdVentaNavigation.Total,
        //                vmc.IdVentaNavigation.IdMesero,
        //                vmc.IdVentaNavigation.IdCliente,
        //                DetalleVenta = vmc.IdVentaNavigation.DetalleVenta.Select(dv => new
        //                {
        //                    dv.IdPlatilloNavigation.Platillo,
        //                    dv.IdPlatilloNavigation.Precio
        //                    //dv.PrecioPlatillo
        //                })
        //            })
        //        })
        //    };

        //    return Ok(reporte);
        //}




        //[HttpGet]
        //public async Task<ActionResult> GetDailyReport()
        //{
        //    // Obtener la fecha actual
        //    //int fechaActual = DateTime.Today;
        //    DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Today);

        //    // Obtener la caja diaria correspondiente a la fecha actual
        //    var cajaDiaria = await _dbContext.CajaDiaria
        //        .Include(cd => cd.MovimientoCajas)
        //            .ThenInclude(mc => mc.IdTipoMovimientoNavigation)
        //        .Include(cd => cd.MovimientoCajas)
        //            .ThenInclude(mc => mc.GastosMovimientoCajas)
        //                .ThenInclude(gmc => gmc.IdGastoNavigation)
        //                    .ThenInclude(g => g.IdProveedorNavigation)
        //        .Include(cd => cd.MovimientoCajas)
        //            .ThenInclude(mc => mc.VentaMovimientoCajas)
        //                .ThenInclude(vmc => vmc.IdVentaNavigation)
        //                    .ThenInclude(v => v.DetalleVenta)
        //        .FirstOrDefaultAsync(cd => cd.Fecha == fechaActual);

        //    if (cajaDiaria == null)
        //    {
        //        return NotFound();
        //    }

        //    // Construir el reporte de caja diario
        //    var reporte = new
        //    {
        //        CajaDiaria = new
        //        {
        //            cajaDiaria.IdCajaDiaria,
        //            cajaDiaria.Fecha,
        //            cajaDiaria.SaldoInicial,
        //            cajaDiaria.SaldoFinal,
        //            cajaDiaria.Estado
        //        },
        //        MovimientosCaja = cajaDiaria.MovimientoCajas.Select(m => new
        //        {
        //            m.IdMovimiento,
        //            TipoMovimiento = m.IdTipoMovimientoNavigation,
        //            m.Concepto,
        //            m.Total,
        //            Gastos = m.GastosMovimientoCajas.Select(gmc => new
        //            {
        //                gmc.IdGastoNavigation.IdGasto,
        //                gmc.IdGastoNavigation.NumeroDocumento,
        //                gmc.IdGastoNavigation.Fecha,
        //                gmc.IdGastoNavigation.Concepto,
        //                gmc.IdGastoNavigation.Total,
        //                Proveedor = gmc.IdGastoNavigation.IdProveedorNavigation
        //            }),
        //            Ventas = m.VentaMovimientoCajas.Select(vmc => new
        //            {
        //                vmc.IdVentaNavigation.IdVenta,
        //                vmc.IdVentaNavigation.NumeroComanda,
        //                vmc.IdVentaNavigation.Fecha,
        //                vmc.IdVentaNavigation.Total,
        //                vmc.IdVentaNavigation.IdMesero,
        //                vmc.IdVentaNavigation.IdCliente,
        //                //DetalleVenta = vmc.IdVentaNavigation.DetalleVenta.Select(dv => new
        //                //{
        //                //    dv.IdPlatilloNavigation.Platillo,

        //                //})
        //            })
        //        })
        //    };

        //    return Ok(reporte);
        //}


        //[HttpGet]
        //public async Task<IActionResult> GenerateDailyReport()
        //{
        //    var cajasDiarias = await 
        //        _dbContext.CajaDiaria
        //        .Include(c => c.MovimientoCajas)
        //            .ThenInclude(mc => mc.VentaMovimientoCajas)
        //                .ThenInclude(vmc => vmc.IdVentaNavigation)
        //        .Include(c => c.MovimientoCajas)
        //            .ThenInclude(mc => mc.GastosMovimientoCajas)
        //                .ThenInclude(gmc => gmc.IdGastoNavigation)
        //        .ToListAsync();

        //    var reportData = new List<DailyReportDto>();

        //    foreach (var cajaDiaria in cajasDiarias)
        //    {
        //        var ventas = cajaDiaria.MovimientoCajas
        //            .SelectMany(mc => mc.VentaMovimientoCajas)
        //            .Select(vmc => new SaleDto
        //            {
        //                IdVenta = vmc.IdVentaNavigation.IdVenta,
        //                NumeroComanda = vmc.IdVentaNavigation.NumeroComanda,
        //                Fecha = vmc.IdVentaNavigation.Fecha,
        //                Total = vmc.IdVentaNavigation.Total,
        //                IdMesero = vmc.IdVentaNavigation.IdMesero,
        //                IdCliente = vmc.IdVentaNavigation.IdCliente,
        //                //DetalleVentas = vmc.IdVentaNavigation.DetalleVenta.Select(dv => dv.Cantidad).ToList(),
        //                DetalleVentas = v.Venta.DetalleVenta.Select(dv => new DetalleVentaDtoOut
        //                { 
        //                }
        //                    //PrecioPlatillos = vmc.IdVentaNavigation.DetalleVenta.Select(dv => dv.IdPlatillo).ToList()
        //                })
        //            .ToList();

        //        var gastos = cajaDiaria.MovimientoCajas
        //            .SelectMany(mc => mc.GastosMovimientoCajas)
        //            .Select(gmc => new ExpenseDto
        //            {
        //                IdGasto = gmc.IdGastoNavigation.IdGasto,
        //                NumeroDocumento = gmc.IdGastoNavigation.NumeroDocumento,
        //                Fecha = gmc.IdGastoNavigation.Fecha,
        //                Concepto = gmc.IdGastoNavigation.Concepto,
        //                Total = gmc.IdGastoNavigation.Total,
        //                IdProveedor = gmc.IdGastoNavigation.IdProveedor,
        //                NombreProveedor = gmc.IdGastoNavigation.IdProveedorNavigation.Nombre
        //            })
        //            .ToList();

        //        var dailyReport = new DailyReportDto
        //        {
        //            IdCajaDiaria = cajaDiaria.IdCajaDiaria,
        //            Fecha = cajaDiaria.Fecha,
        //            SaldoInicial = cajaDiaria.SaldoInicial,
        //            SaldoFinal = cajaDiaria.SaldoFinal,
        //            Estado = cajaDiaria.Estado,
        //            Ventas = ventas,
        //            Gastos = gastos
        //        };

        //        reportData.Add(dailyReport);
        //    }

        //    return Ok(reportData);
        //}


        //[HttpGet("daily")]
        //public IActionResult GenerateDailyReport()
        //{

        //    //DateOnly today = DateOnly.FromDateTime(new DateTime(2023, 3, 3));
        //    // Obtener la fecha actual
        //    DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);

        //    // Obtener la caja diaria correspondiente a la fecha actual
        //    CajaDiaria cajaDiaria = _dbContext.CajaDiaria.FirstOrDefault(c => c.Fecha == currentDate);

        //    if (cajaDiaria == null)
        //    {
        //        return NotFound("No se encontró la caja diaria para la fecha actual.");
        //    }

        //    // Obtener los movimientos de caja relacionados con la caja diaria actual
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

        //    // Crear el objeto de respuesta
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
        //        MovimientosCaja = movimientosCaja
        //    };

        //    return Ok(response);
        //}

        //[HttpGet("daily")]
        ////public async IActionResult GenerateDailyReport()

        //public async Task<IEnumerable<CajaDtoOut>> GetAll()
        //{
        //        // Obtener la fecha actual
        //        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Today);

        //        // Obtener la caja diaria correspondiente a la fecha actual
        //        CajaDiaria cajaDiaria = _dbContext.CajaDiaria.FirstOrDefault(c => c.Fecha == currentDate);

        //        //if (cajaDiaria == null)
        //        //{
        //        //    return NotFound("No se encontró la caja diaria para la fecha actual.");
        //        //}

        //        // Obtener los movimientos de caja relacionados con la caja diaria actual
        //        //var movimientosCaja = (from mc in _dbContext.MovimientoCajas
        //        //                       where mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria
        //        //                       select new
        //        //                       {
        //        //                           mc.IdMovimiento,
        //        //                           mc.IdTipoMovimiento,
        //        //                           mc.Concepto,
        //        //                           mc.Total,
        //        //                           Ventas = (from vmc in _dbContext.VentaMovimientoCajas
        //        //                                     join v in _dbContext.Ventas on vmc.IdVenta equals v.IdVenta
        //        //                                     where vmc.IdMovimientoCaja == mc.IdMovimiento
        //        //                                     select new
        //        //                                     {
        //        //                                         v.IdVenta,
        //        //                                         v.NumeroComanda,
        //        //                                         v.Fecha,
        //        //                                         v.Total,
        //        //                                         v.IdMesero,
        //        //                                         v.IdCliente
        //        //                                     }).ToList(),
        //        //                           Gastos = (from gmc in _dbContext.GastosMovimientoCajas
        //        //                                     join g in _dbContext.Gastos on gmc.IdGasto equals g.IdGasto
        //        //                                     where gmc.IdMovimientoCaja == mc.IdMovimiento
        //        //                                     select new
        //        //                                     {
        //        //                                         g.IdGasto,
        //        //                                         g.NumeroDocumento,
        //        //                                         g.Fecha,
        //        //                                         g.Concepto,
        //        //                                         g.Total,
        //        //                                         g.IdProveedor
        //        //                                     }).ToList()
        //        //                       }).ToList();

        //        var movimientosCaja = await _dbContext.MovimientoCajas
        //.Where(mc => mc.IdCajaDiaria == cajaDiaria.IdCajaDiaria)
        //.Select(mc => new
        //{
        //    mc.IdMovimiento,
        //    mc.IdTipoMovimiento,
        //    mc.Concepto,
        //    mc.Total,
        //    Ventas = _dbContext.VentaMovimientoCajas
        //        .Where(vmc => vmc.IdMovimientoCaja == mc.IdMovimiento)
        //        .Join(_dbContext.Ventas,
        //            vmc => vmc.IdVenta,
        //            v => v.IdVenta,
        //            (vmc, v) => new
        //            {
        //                v.IdVenta,
        //                v.NumeroComanda,
        //                v.Fecha,
        //                v.Total,
        //                v.IdMesero,
        //                v.IdCliente
        //            })
        //        .ToListAsync(),
        //    Gastos = _dbContext.GastosMovimientoCajas
        //        .Where(gmc => gmc.IdMovimientoCaja == mc.IdMovimiento)
        //        .Join(_dbContext.Gastos,
        //            gmc => gmc.IdGasto,
        //            g => g.IdGasto,
        //            (gmc, g) => new
        //            {
        //                g.IdGasto,
        //                g.NumeroDocumento,
        //                g.Fecha,
        //                g.Concepto,
        //                g.Total,
        //                g.IdProveedor
        //            })
        //        .ToListAsync()
        //})
        //.ToListAsync();


        //        // Agrupar los gastos y ventas por tipo de movimiento
        //        //var gastosAgrupados = movimientosCaja
        //        //    .SelectMany(mc => mc.Gastos)
        //        //    .GroupBy(g => g.IdGasto)
        //        //    .Select(g => new
        //        //    {
        //        //        IdTipoMovimiento = g.Key,
        //        //        Gastos = g.ToList()
        //        //    })
        //        //    .ToList();

        //        //var ventasAgrupadas = movimientosCaja
        //        //    .SelectMany(mc => mc.Ventas)
        //        //    .GroupBy(v => v.IdVenta)
        //        //    .Select(v => new
        //        //    {
        //        //        IdTipoMovimiento = v.Key,
        //        //        Ventas = v.ToList()
        //        //    })
        //        //    .ToList();
        //        if (movimientosCaja == null)
        //        {
        //            return NotFound("No se encontraron datos para el año actual.");
        //        }
        //        // Crear el objeto de respuesta
        //        //var response = new
        //        //{
        //        //    CajaDiaria = new
        //        //    {
        //        //        cajaDiaria.IdCajaDiaria,
        //        //        cajaDiaria.Fecha,
        //        //        cajaDiaria.SaldoInicial,
        //        //        cajaDiaria.SaldoFinal,
        //        //        cajaDiaria.Estado
        //        //    },
        //        //    MovimientosCaja = movimientosCaja.Select(mc => new
        //        //    {
        //        //        mc.IdMovimiento,
        //        //        mc.IdTipoMovimiento,
        //        //        mc.Concepto,
        //        //        mc.Total,
        //        //        //Ventas = ventasAgrupadas.FirstOrDefault(v => v.IdTipoMovimiento == mc.IdTipoMovimiento)?.Ventas,
        //        //        //Gastos = gastosAgrupados.FirstOrDefault(g => g.IdTipoMovimiento == mc.IdTipoMovimiento)?.Gastos
        //        //    }).ToList()
        //        //};

        //        //return Ok(movimientosCaja);
        //        return movimientosCaja;


        //}




    }
}
