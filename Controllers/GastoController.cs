using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
    public class GastoController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public GastoController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<GastoDtoOut>> GetAll()
        {
            try
            {
                var gasto = await _dbContext.Gastos
                    .Include(p => p.IdProveedorNavigation)
                    .ToListAsync();

                var gastoDtoOut = gasto.Select(p => new GastoDtoOut
                {
                    IdGasto = p.IdGasto,
                    NumeroDocumento = p.NumeroDocumento,
                    Fecha = p.Fecha,
                    Concepto = p.Concepto,
                    Total = p.Total,
                    Proveedor = p.IdProveedorNavigation != null ? 
                    p.IdProveedorNavigation.Nombre : string.Empty
                }).ToList().OrderByDescending(g => g.IdGasto);

                return Ok(gastoDtoOut);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error al obtener los gastos", ex });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [Route("{idGasto:long}")]
        public async Task<ActionResult<GastoDtoOut>> GetById(long idGasto)
        {
            try
            {
                var gasto = await _dbContext.Gastos
                .Include(p => p.IdProveedorNavigation)
                .FirstOrDefaultAsync(g => g.IdGasto == idGasto);

                if (gasto == null)
                {
                    return NotFound(new { message = "Gasto no encontrado" });
                }

                var gastoDtoOut = new GastoDtoOut
                {
                    IdGasto = gasto.IdGasto,
                    NumeroDocumento = gasto.NumeroDocumento,
                    Fecha = gasto.Fecha,
                    Concepto = gasto.Concepto,
                    Total = gasto.Total,
                    Proveedor = gasto.IdProveedorNavigation != null ?
                    gasto.IdProveedorNavigation.Nombre : string.Empty
                };

                return gastoDtoOut;
            } catch (Exception ex)
            {
                return BadRequest(new { message = "Error al obtener los gastos", ex });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult<GastoDtoOut>> Create(GastoDtoIn gastoDtoIn)
        {
            if (gastoDtoIn == null)
            {
                return BadRequest();
            }

            var gasto = new Gasto
            {
                NumeroDocumento = gastoDtoIn.NumeroDocumento,
                Fecha = gastoDtoIn.Fecha,
                Concepto = gastoDtoIn.Concepto,
                Total = gastoDtoIn.Total,
                IdProveedor = gastoDtoIn.IdProveedor
            };

            // Comprueba si se proporcionó un ID de proveedor válido
            if (gastoDtoIn.IdProveedor.HasValue)
            {
                var proveedor = await _dbContext.Proveedores.FindAsync(gastoDtoIn.IdProveedor.Value);
                if (proveedor == null)
                {
                    return BadRequest("El ID del proveedor no es válido");
                }

                gasto.IdProveedorNavigation = proveedor;
            }

            _dbContext.Gastos.Add(gasto);
            await _dbContext.SaveChangesAsync();

            var gastoDtoOut = new GastoDtoOut
            {
                IdGasto = gasto.IdGasto,
                NumeroDocumento = gasto.NumeroDocumento,
                Fecha = gasto.Fecha,
                Concepto = gasto.Concepto,
                Total = gasto.Total,
                Proveedor = gasto.IdProveedorNavigation != null ? gasto.IdProveedorNavigation.Nombre : string.Empty
            };

            return CreatedAtAction(nameof(GetById), new { id = gasto.IdGasto }, gastoDtoOut);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("{idGasto:long}")]
        public async Task<IActionResult> Update(long idGasto, GastoDtoIn gastoDto)
        {
            var gastoToUpdate = await _dbContext.Gastos.FindAsync(idGasto);

            if (gastoToUpdate is null)
            {
                return NotFound(new { message = "Gasto no encontrado" });
            }

            // Actualizar solo los campos que se proporcionan en el DTO
            gastoToUpdate.NumeroDocumento = gastoDto.NumeroDocumento ?? gastoToUpdate.NumeroDocumento;
            gastoToUpdate.Fecha = gastoDto.Fecha ?? gastoToUpdate.Fecha;
            gastoToUpdate.Concepto = gastoDto.Concepto ?? gastoToUpdate.Concepto;
            gastoToUpdate.Total = gastoDto.Total ?? gastoToUpdate.Total;
            gastoToUpdate.IdProveedor = gastoDto.IdProveedor ?? gastoToUpdate.IdProveedor;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{idGasto:long}")]
        public async Task<IActionResult> Delete(long idGasto)
        {
            var gastoToDelete = await _dbContext.Gastos.FindAsync(idGasto);

            if (gastoToDelete is null)
            {
                return NotFound(new { message = "Gasto no encontrado" });
            }

            _dbContext.Gastos.Remove(gastoToDelete);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
