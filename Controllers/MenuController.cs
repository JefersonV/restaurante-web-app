using Microsoft.AspNetCore.Mvc;
using restaurante_web_app.Models;
using Microsoft.AspNetCore.Cors;//para hacer uso del CORS
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace restaurante_web_app.Controllers
{
    //añadir las reglas de cors dentro de cada controlador
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public MenuController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Authorize(Roles = "Administrador, Invitado")]
        [HttpGet]
        public async Task<IEnumerable<Menu>> GetAll()
        {
            //return await _dbContext.Menus.Include(d => d.DetalleVenta).ToListAsync();
            return await _dbContext.Menus.OrderByDescending(m => m.IdPlatillo).ToListAsync();
        }

        [Authorize(Roles = "Administrador, Invitado")]
        [HttpGet]
        [Route("{idPlatillo:int}")]
        public async Task<ActionResult<Menu>> GetById(int idPlatillo)
        {
            var menu = await _dbContext.Menus.FindAsync(idPlatillo);

            if (menu is null)
                return NotFound(new { message = "Platillo no encontrado" });
            return menu;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<Menu> Create(Menu newMenu)
        {
            _dbContext.Menus.Add(newMenu);
            await _dbContext.SaveChangesAsync();

            return newMenu;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        [Route("{idMenu:int}")]
        public async Task<IActionResult> Update(int idMenu, Menu menu)
        {
            if (idMenu != menu.IdPlatillo)
                return BadRequest("Platillo no encontrado");

            var menuToUpdate = await _dbContext.Menus.FindAsync(idMenu);

            if (menuToUpdate is not null)
            {
                menuToUpdate.Platillo = string.IsNullOrEmpty(menu.Platillo) ? 
                    menuToUpdate.Platillo : menu.Platillo;
                menuToUpdate.Precio = menu.Precio == 0 ? menuToUpdate.Precio : menu.Precio;
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound(new { message = "Platillo no encontrado" });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete]
        [Route("{idMenu:int}")]
        public async Task<IActionResult> Delete(int idMenu)
        {
            var menuToDelete = await _dbContext.Menus.FindAsync(idMenu);
            if (menuToDelete is not null)
            {
                _dbContext.Menus.Remove(menuToDelete);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound(new { message = "Platillo no encontrado" });
            }
        }
    }
}
