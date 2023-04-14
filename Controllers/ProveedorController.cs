﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Models;

namespace restaurante_web_app.Controllers
{
    //añadir las reglas de cors dentro de cada controlador
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public ProveedorController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Proveedor>> GetAll()
        {
            return await _dbContext.Proveedores.ToListAsync();
        }

        [HttpGet]
        [Route("{idProveedor:int}")]
        public async Task<ActionResult<Proveedor>> GetById(int idProveedor)
        {
            var proveedor = await _dbContext.Proveedores.FindAsync(idProveedor);

            if (proveedor is null)
                return NotFound(new { message = "Proveedor no encontrado" });
            return proveedor;
        }

        [HttpPost]
        public async Task<Proveedor> Create(Proveedor newProveedor)
        {
            _dbContext.Proveedores.Add(newProveedor);
            await _dbContext.SaveChangesAsync();

            return newProveedor;
        }

        [HttpPut]
        [Route("{idProveedor:int}")]
        public async Task<IActionResult> Update(int idProveedor, Proveedor proveedor)
        {
            if (idProveedor != proveedor.IdProveedor)
                return BadRequest("Proveedor no encontrado");
            var proveedorToUpdate = await _dbContext.Proveedores.FindAsync(idProveedor);

            if (proveedorToUpdate is not null)
            {
                proveedorToUpdate.Nombre = proveedor.Nombre;
                proveedorToUpdate.Telefono = proveedor.Telefono;
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound(new { message = "Proveedor no encontrado" });
            }
        }

        [HttpDelete]
        [Route("{idProveedor:int}")]

        public async Task<IActionResult> Delete(int idProveedor)
        {
            var proveedorToDelete = await _dbContext.Proveedores.FindAsync(idProveedor);
            if (proveedorToDelete is not null)
            {
                _dbContext.Proveedores.Remove(proveedorToDelete);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound(new { message = "Proveedor no encontrado" });
            }
        }
    }
}
