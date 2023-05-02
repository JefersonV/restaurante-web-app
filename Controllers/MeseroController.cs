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
    public class MeseroController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public MeseroController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Administrador, Invitado")]
        [HttpGet]
        public async Task<IEnumerable<Mesero>> GetAll()
        {
            return await _dbContext.Meseros.OrderByDescending(m => m.IdMesero).ToListAsync();
        }

        [Authorize(Roles = "Administrador, Invitado")]
        [HttpGet]
        [Route("{idMesero:int}")]
        public async Task<ActionResult<Mesero>> GetById(int idMesero)
        {
            var mesero = await _dbContext.Meseros.FindAsync(idMesero);

            if (mesero is null)
                return NotFound(new { message = "Mesero no encontrado" });
            return mesero;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<Mesero> Create(Mesero newMesero)
        {
            _dbContext.Meseros.Add(newMesero);
            await _dbContext.SaveChangesAsync();

            return newMesero;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        [Route("{idMesero:int}")]
        public async Task<IActionResult> Update(int idMesero, Mesero mesero)
        {
            if (idMesero != mesero.IdMesero)
                return BadRequest("Mesero no encontrado");
            var meseroToUpdate = await _dbContext.Meseros.FindAsync(idMesero);

            if (meseroToUpdate is not null)
            {
                meseroToUpdate.Nombre = mesero.Nombre;
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound(new { message = "Mesero no encontrado" });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete]
        [Route("{idMesero:int}")]
        public async Task<IActionResult> Delete(int idMesero)
        {
            var meseroToDelete = await _dbContext.Meseros.FindAsync(idMesero);
            if (meseroToDelete is not null)
            {
                _dbContext.Meseros.Remove(meseroToDelete);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound(new { message = "Mesero no encontrado" });
            }
        }
    }
}
