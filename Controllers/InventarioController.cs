using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
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
    public class InventarioController : ControllerBase
    {
        public readonly GoeatContext _dbContext;

        public InventarioController(GoeatContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        [HttpGet]
        public async Task<IEnumerable<InventarioDtoOut>> GetAll()
        {
            var fechaActual = DateOnly.FromDateTime(DateTime.Now);
            var inicioMes = new DateOnly(fechaActual.Year, fechaActual.Month, 1);
            var finMes = inicioMes.AddMonths(1).AddDays(-1);

            var inventario = await _dbContext.MovimientoCajas
                .Include(m => m.IdTipoMovimientoNavigation)
                .Include(m => m.IdCajaDiariaNavigation)
                .Where(m => m.IdCajaDiariaNavigation.Fecha >= inicioMes && m.IdCajaDiariaNavigation.Fecha <= finMes)
                .OrderByDescending(m => m.IdMovimiento)
                .Select(m => new InventarioDtoOut
                {
                    IdMovimiento = m.IdMovimiento,
                    TipoMovimiento = m.IdTipoMovimientoNavigation.Tipo,
                    FechaCaja = m.IdCajaDiariaNavigation.Fecha,
                    Total = m.Total
                })
                .ToListAsync();

            return inventario;
        }

    }
}
