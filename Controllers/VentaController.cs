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
    public class VentaController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public VentaController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }
        //Obtener todas las ventas con todos sus detalles
        [HttpGet]
        public async Task<IEnumerable<VentaDtoOut>> GetAll()
        {
            var ventas = await _dbContext.Ventas
                .Include(v => v.DetalleVenta)
                .ThenInclude(dv => dv.IdPlatilloNavigation)
                .Join(_dbContext.Meseros,
                    v => v.IdMesero,
                    m => m.IdMesero,
                    (v, m) => new { Venta = v, Mesero = m })
                .Join(_dbContext.Clientes,
                    vm => vm.Venta.IdCliente,
                    c => c.IdCliente,
                    (vm, c) => new { Venta = vm.Venta, Mesero = vm.Mesero, Cliente = c })
                .ToListAsync();

            var ventasDto = ventas.Select(v => new VentaDtoOut
            {
                IdVenta = v.Venta.IdVenta,
                NumeroComanda = v.Venta.NumeroComanda,
                Fecha = v.Venta.Fecha,
                Total = v.Venta.Total,
                Mesero = v.Mesero.Nombre,
                Cliente = v.Cliente.NombreApellido,
                DetalleVenta = v.Venta.DetalleVenta.Select(dv => new DetalleVentaDtoOut
                {
                    Id = dv.IdDetalleVenta,
                    Platillo = dv.IdPlatilloNavigation?.Platillo,
                    IdVenta = dv.IdVenta,
                    Cantidad = dv.Cantidad,
                    Subtotal = dv.Subtotal,
                    Observaciones = dv.Observaciones,
                }).ToList(),
            });

            return ventasDto;
        }
        //Obtener una unica venta con sus detalles
        [HttpGet("{id:long}")]
        public async Task<ActionResult<VentaDtoOut>> GetById(long id)
        {
            var venta = await _dbContext.Ventas
                .Where(v => v.IdVenta == id)
                .Include(v => v.DetalleVenta)
                .ThenInclude(dv => dv.IdPlatilloNavigation)
                .Join(_dbContext.Meseros,
                    v => v.IdMesero,
                    m => m.IdMesero,
                    (v, m) => new { Venta = v, Mesero = m })
                .Join(_dbContext.Clientes,
                    vm => vm.Venta.IdCliente,
                    c => c.IdCliente,
                    (vm, c) => new { Venta = vm.Venta, Mesero = vm.Mesero, Cliente = c })
                .FirstOrDefaultAsync();

            if (venta == null)
            {
                return NotFound();
            }

            var ventaDto = new VentaDtoOut
            {
                IdVenta = venta.Venta.IdVenta,
                NumeroComanda = venta.Venta.NumeroComanda,
                Fecha = venta.Venta.Fecha,
                Total = venta.Venta.Total,
                Mesero = venta.Mesero.Nombre,
                Cliente = venta.Cliente.NombreApellido,
                DetalleVenta = venta.Venta.DetalleVenta.Select(dv => new DetalleVentaDtoOut
                {
                    Id = dv.IdDetalleVenta,
                    Platillo = dv.IdPlatilloNavigation?.Platillo,
                    IdVenta = dv.IdVenta,
                    Cantidad = dv.Cantidad,
                    Subtotal = dv.Subtotal,
                    Observaciones = dv.Observaciones,
                }).ToList(),
            };

            return ventaDto;
        }
        //Crear una venta luego crear los detalles que contiene con su relacion uno a muchos
        [HttpPost]
        public async Task<ActionResult<VentaDtoOut>> Create(VentaDtoIn ventaDto)
        {
            var venta = new Venta
            {
                NumeroComanda = ventaDto.NumeroComanda,
                //Fecha = ventaDto.Fecha.HasValue ? ventaDto.Fecha.Value : DateOnly.FromDateTime(DateTime.Now.Date),
                Fecha = ventaDto.Fecha.HasValue ? ventaDto.Fecha.Value : default(DateOnly?),
                Total = ventaDto.Total ?? 0,
                IdMesero = ventaDto.IdMesero ?? 0,
                IdCliente = ventaDto.IdCliente ?? 0
            };

            _dbContext.Ventas.Add(venta);
            await _dbContext.SaveChangesAsync();

            if (ventaDto.DetalleVenta != null && ventaDto.DetalleVenta.Any())
            {
                var detalleVenta = ventaDto.DetalleVenta.Select(dvDto => new DetalleVenta
                {
                    Cantidad = dvDto.Cantidad ?? 0,
                    Subtotal = dvDto.Subtotal ?? 0,
                    Observaciones = dvDto.Observaciones,
                    IdPlatillo = dvDto.IdPlatillo,
                    IdVenta = venta.IdVenta
                }).ToList();

                _dbContext.DetalleVenta.AddRange(detalleVenta);
                await _dbContext.SaveChangesAsync();
            }

            var ventaOut = await _dbContext.Ventas
                .Include(v => v.DetalleVenta)
                .ThenInclude(dv => dv.IdPlatilloNavigation)
                .Join(_dbContext.Meseros,
                    v => v.IdMesero,
                    m => m.IdMesero,
                    (v, m) => new { Venta = v, Mesero = m })
                .Join(_dbContext.Clientes,
                    vm => vm.Venta.IdCliente,
                    c => c.IdCliente,
                    (vm, c) => new { Venta = vm.Venta, Mesero = vm.Mesero, Cliente = c })
                .FirstOrDefaultAsync(v => v.Venta.IdVenta == venta.IdVenta);

            var ventaDtoOut = new VentaDtoOut
            {
                IdVenta = ventaOut.Venta.IdVenta,
                NumeroComanda = ventaOut.Venta.NumeroComanda,
                Fecha = ventaOut.Venta.Fecha,
                Total = ventaOut.Venta.Total,
                Mesero = ventaOut.Mesero.Nombre,
                Cliente = ventaOut.Cliente.NombreApellido,
                DetalleVenta = ventaOut.Venta.DetalleVenta.Select(dv => new DetalleVentaDtoOut
                {
                    Id = dv.IdDetalleVenta,
                    Platillo = dv.IdPlatilloNavigation?.Platillo,
                    IdVenta = dv.IdVenta,
                    Cantidad = dv.Cantidad,
                    Subtotal = dv.Subtotal,
                    Observaciones = dv.Observaciones,
                }).ToList(),
            };

            return CreatedAtAction(nameof(GetById), new { id = ventaDtoOut.IdVenta }, ventaDtoOut);
        }
        //Actualizar los detalles de una venta 
        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateDetalleVenta(long id, List<DetalleVentaDtoIn> detalleVentaDto)
        {
            var venta = await _dbContext.Ventas
                .Include(v => v.DetalleVenta)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null)
            {
                return NotFound();
            }

            // Actualizar detalles de venta existentes o agregar nuevos
            foreach (var dvDto in detalleVentaDto)
            {
                var dv = venta.DetalleVenta.FirstOrDefault(d => d.IdDetalleVenta == dvDto.IdDetalleVenta);

                if (dv != null)
                {
                    dv.Cantidad = dvDto.Cantidad ?? 0;
                    dv.Subtotal = dvDto.Subtotal ?? 0;
                    dv.Observaciones = dvDto.Observaciones;
                    dv.IdPlatillo = dvDto.IdPlatillo;
                }
                else
                {
                    venta.DetalleVenta.Add(new DetalleVenta
                    {
                        Cantidad = dvDto.Cantidad ?? 0,
                        Subtotal = dvDto.Subtotal ?? 0,
                        Observaciones = dvDto.Observaciones,
                        IdPlatillo = dvDto.IdPlatillo,
                    });
                }
            }

            // Guardar cambios
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        //Eliminar detalles de una venta -> no elimina la venta solo los detalles que se indique
        [HttpDelete("{idVenta:long}/detalleVenta/{idDetalleVenta:long}")]
        public async Task<ActionResult> DeleteDetalleVenta(long idVenta, long idDetalleVenta)
        {
            var venta = await _dbContext.Ventas
                .Include(v => v.DetalleVenta)
                .FirstOrDefaultAsync(v => v.IdVenta == idVenta);

            if (venta == null)
            {
                return NotFound();
            }

            var detalleVenta = venta.DetalleVenta.FirstOrDefault(dv => dv.IdDetalleVenta == idDetalleVenta);

            if (detalleVenta == null)
            {
                return NotFound();
            }

            venta.DetalleVenta.Remove(detalleVenta);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        //Eliminar una venta y los detalles que lo conforman
        [HttpDelete("{id:long}")]
        public async Task<ActionResult> DeleteVenta(long id)
        {
            var venta = await _dbContext.Ventas
                .Include(v => v.DetalleVenta)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null)
            {
                return NotFound();
            }

            // Eliminar los detalles de venta relacionados a la venta
            _dbContext.DetalleVenta.RemoveRange(venta.DetalleVenta);

            // Eliminar la venta
            _dbContext.Ventas.Remove(venta);

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
