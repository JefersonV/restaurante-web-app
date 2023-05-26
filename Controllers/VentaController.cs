using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Data.DTOs;
using restaurante_web_app.Models;
using System.Data;

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

        [Authorize(Roles = "Administrador, Invitado")]
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
                    Precio = dv.IdPlatilloNavigation?.Precio ?? 0,
                    IdVenta = dv.IdVenta,
                    Cantidad = dv.Cantidad,
                    Subtotal = dv.Subtotal,
                    Observaciones = dv.Observaciones,
                }).ToList(),
            }).OrderByDescending(v => v.IdVenta);

            return ventasDto;
        }

        [Authorize(Roles = "Administrador, Invitado")]
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
                    Precio = dv.IdPlatilloNavigation?.Precio ?? 0,
                    IdVenta = dv.IdVenta,
                    Cantidad = dv.Cantidad,
                    Subtotal = dv.Subtotal,
                    Observaciones = dv.Observaciones,
                }).ToList(),
            };

            return ventaDto;
        }

        [Authorize(Roles = "Administrador, Invitado")]
        //Crear una venta luego crear los detalles que contiene con su relacion uno a muchos
        [HttpPost]
        public async Task<ActionResult<VentaDtoOut>> Create(VentaDtoIn ventaDto)
        {
#pragma warning disable CS8604 // Posible argumento de referencia nulo
            var total = ventaDto.DetalleVenta.Sum(dvDto => dvDto.Cantidad * _dbContext.Menus
            .Single(p => p.IdPlatillo == dvDto.IdPlatillo).Precio);
#pragma warning restore CS8604 // Posible argumento de referencia nulo

            var venta = new Venta
            {
                NumeroComanda = ventaDto.NumeroComanda,
                Fecha = ventaDto.Fecha.HasValue ? ventaDto.Fecha.Value : default(DateOnly?),
                Total = total,
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
                    Subtotal = (dvDto.Cantidad ?? 0) * _dbContext.Menus
                    .Single(p => p.IdPlatillo == dvDto.IdPlatillo).Precio,
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


            if (ventaOut == null)
            {
                return NotFound();
            }

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

        [Authorize(Roles = "Administrador")]
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
                    // Conserva el valor actual si no está presente en el objeto JSON
                    dv.Cantidad = dvDto.Cantidad ?? dv.Cantidad;
                    dv.Observaciones = dvDto.Observaciones ?? dv.Observaciones;
                    dv.IdPlatillo = dvDto.IdPlatillo ?? dv.IdPlatillo;

                    // Calcular nuevo subtotal
                    var platillo = await _dbContext.Menus.FindAsync(dv.IdPlatillo);
                    if (platillo != null)
                    {
                        dv.Subtotal = dv.Cantidad * platillo.Precio;
                    }
                }
                else
                {
                    var platillo = await _dbContext.Menus.FindAsync(dvDto.IdPlatillo);
                    if (platillo != null)
                    {
                        venta.DetalleVenta.Add(new DetalleVenta
                        {
                            Cantidad = dvDto.Cantidad ?? 0,
                            Subtotal = dvDto.Cantidad * platillo.Precio,
                            Observaciones = dvDto.Observaciones,
                            IdPlatillo = dvDto.IdPlatillo,
                        });
                    }
                }
            }

            // Calcular nuevo total de venta
            venta.Total = venta.DetalleVenta.Sum(dv => dv.Subtotal);

            // Guardar cambios
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Administrador")]
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

            decimal? subtotalDetalleVenta = detalleVenta.Subtotal;

            venta.DetalleVenta.Remove(detalleVenta);

            venta.Total -= subtotalDetalleVenta;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Administrador")]
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
