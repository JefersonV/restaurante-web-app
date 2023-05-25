using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Models;
using System.Data;

namespace restaurante_web_app.Controllers
{
    //añadir las reglas de cors dentro de cada controlador
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public ClienteController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Administrador, Invitado")]
        [HttpGet]
        public async Task<IEnumerable<Cliente>> GetAll()
        {
            return await _dbContext.Clientes.OrderByDescending(c => c.IdCliente).ToListAsync();
        }

        [Authorize(Roles = "Administrador, Invitado")]
        [HttpGet]
        [Route("{idCliente:int}")]
        public async Task<ActionResult<Cliente>> GetById(int idCliente)
        {
            var cliente = await _dbContext.Clientes.FindAsync(idCliente);

            if (cliente is null)
                return NotFound(new { message = "Cliente no encontrado" });
            return cliente;
        }

        [Authorize(Roles = "Administrador, Invitado")]
        [HttpPost]
        public async Task<Cliente> Create(Cliente newCliente)
        {
            _dbContext.Clientes.Add(newCliente);
            await _dbContext.SaveChangesAsync();

            return newCliente;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        [Route("{idCliente:int}")]
        public async Task<IActionResult> Update(int idCliente, Cliente cliente)
        {
            if (idCliente != cliente.IdCliente)
                return BadRequest("Cliente no encontrado");
            var clienteToUpdate = await _dbContext.Clientes.FindAsync(idCliente);

            if (clienteToUpdate is not null)
            {
                clienteToUpdate.NombreApellido = string.IsNullOrWhiteSpace(cliente.NombreApellido) ? 
                    clienteToUpdate.NombreApellido : cliente.NombreApellido;
                clienteToUpdate.Fecha = cliente.Fecha ?? clienteToUpdate.Fecha;
                clienteToUpdate.Institucion = string.IsNullOrWhiteSpace(cliente.Institucion) ? 
                    clienteToUpdate.Institucion : cliente.Institucion;
                clienteToUpdate.Puesto = string.IsNullOrWhiteSpace(cliente.Puesto) ? 
                    clienteToUpdate.Puesto : cliente.Puesto;
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound(new { message = "Cliente no encontrado" });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete]
        [Route("{idCliente:int}")]

        public async Task<IActionResult> Delete(int idCliente)
        {
            var clienteToDelete = await _dbContext.Clientes.FindAsync(idCliente);
            if (clienteToDelete is not null)
            {
                _dbContext.Clientes.Remove(clienteToDelete);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound(new { message = "Cliente no encontrado" });
            }
        }
    }
}
