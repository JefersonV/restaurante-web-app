using Microsoft.AspNetCore.Mvc;
using restaurante_web_app.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using restaurante_web_app.Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Cors;

namespace restaurante_web_app.Controllers
{
    //añadir las reglas de cors dentro de cada controlador
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly GoeatContext _dbContext;
        private readonly IConfiguration _config;

        public AccountController(GoeatContext _context, IConfiguration config)
        {
            _dbContext = _context;
            _config = config;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("register")]
        public async Task<ActionResult<Usuario>> Register(UsuarioDtoIn usuarioDtoIn)
        {
            try
            {
                // Verificar si el usuario ya existe
                var existingUser = await _dbContext.Usuarios
                    .FirstOrDefaultAsync(u => u.Usuario1 == GenerateUsername(usuarioDtoIn.Nombre));
                if (existingUser != null)
                {
                    return BadRequest("El usuario ya existe");
                }

                // Registrar un nuevo usuario
                var usuario = new Usuario
                {
                    Nombre = usuarioDtoIn.Nombre,
                    Usuario1 = GenerateUsername(usuarioDtoIn.Nombre),
                    Contrasenia = BCrypt.Net.BCrypt.HashPassword(usuarioDtoIn.Contrasenia),
                    IdTipoUsuario = usuarioDtoIn.IdTipoUsuario
                };

                _dbContext.Usuarios.Add(usuario);
                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "Usuario creado exitosamente", usuario = usuario.Usuario1 });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Método para generar el nombre de usuario
        // Solo une el nombre con el apellido todo en minúsculas
        private string GenerateUsername(string nombre)
        {
            var username = nombre.Replace(" ", "").ToLower();
            return username;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDtoIn loginDtoIn)
        {
            try
            {
                var usuario = await _dbContext.Usuarios
                    .Include(u => u.IdTipoUsuarioNavigation)
                    .FirstOrDefaultAsync(u => u.Usuario1 == loginDtoIn.Usuario);
                if (usuario == null)
                {
                    return BadRequest(new { mensaje = "Usuario o contraseña incorrectos" });
                }

                if (!BCrypt.Net.BCrypt.Verify(loginDtoIn.Contrasenia, usuario.Contrasenia))
                {
                    return BadRequest(new { mensaje = "Usuario o contraseña incorrectos" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? string.Empty);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _config["Jwt:Issuer"], // usar el valor del emisor de la configuración.
                    Audience = _config["Jwt:Audience"], // usar el valor de la audiencia de la configuración
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, usuario.Usuario1),
                new Claim(ClaimTypes.Role, usuario.IdTipoUsuarioNavigation?.Tipo ?? "")
                    }),
                    Expires = DateTime.UtcNow.AddHours(10),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                var rol = usuario.IdTipoUsuarioNavigation?.Tipo ?? "";
                var nombreUsuario = usuario.Usuario1;

                return Ok(new { token = tokenString, rol, nombreUsuario });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error en el servidor:", ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDtoOut>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _dbContext.Usuarios
                    .Include(u => u.IdTipoUsuarioNavigation).ToListAsync();

                var usuariosDtoOut = usuarios.Select(usuario =>
                    new UsuarioDtoOut
                    {
                        IdUsuario = usuario.IdUsuario,
                        Nombre = usuario.Nombre,
                        Usuario1 = usuario.Usuario1,
                        TipoUsuario = usuario.IdTipoUsuarioNavigation?.Tipo ?? ""
                    }).OrderByDescending(u => u.IdUsuario).ToList();

                return Ok(usuariosDtoOut);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error en el servidor", ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDtoOut>> GetUsuario(Guid id)
        {
            try
            {
                var usuario = await _dbContext.Usuarios
                .Include(u => u.IdTipoUsuarioNavigation)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

                if (usuario == null)
                {
                    return NotFound();
                }

                var usuarioDtoOut = new UsuarioDtoOut
                {
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    Usuario1 = usuario.Usuario1,
                    TipoUsuario = usuario.IdTipoUsuarioNavigation?.Tipo ?? ""
                };

                return usuarioDtoOut;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Usuario>> UpdateUsuario(Guid id, UsuarioDtoIn usuarioDtoIn)
        {
            try
            {
                // Buscar el usuario existente por su ID
                var usuario = await _dbContext.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                // Verificar si el nombre de usuario ya está en uso por otro usuario
                var existingUser = await _dbContext.Usuarios
                    .FirstOrDefaultAsync(u => u.IdUsuario != id && u.Usuario1 == GenerateUsername(usuarioDtoIn.Nombre));
                if (existingUser != null)
                {
                    return BadRequest("El nombre de usuario ya está en uso");
                }

                // Actualizar los campos del usuario solo si se proporcionan en la solicitud
                usuario.Nombre = !string.IsNullOrEmpty(usuarioDtoIn.Nombre) ? 
                    usuarioDtoIn.Nombre : usuario.Nombre;
                usuario.Contrasenia = !string.IsNullOrEmpty(usuarioDtoIn.Contrasenia) ? 
                    BCrypt.Net.BCrypt.HashPassword(usuarioDtoIn.Contrasenia) : usuario.Contrasenia;
                usuario.IdTipoUsuario = usuarioDtoIn.IdTipoUsuario != 0 ? 
                    usuarioDtoIn.IdTipoUsuario : usuario.IdTipoUsuario;
                usuario.Usuario1 = !string.IsNullOrEmpty(usuarioDtoIn.Nombre) ? 
                    GenerateUsername(usuarioDtoIn.Nombre) : usuario.Usuario1;

                await _dbContext.SaveChangesAsync();

                return Ok(new { message = "Usuario actualizado exitosamente", usuario = usuario.Usuario1 });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(Guid id)
        {
            try
            {
                var usuario = await _dbContext.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    return NotFound();
                }

                _dbContext.Usuarios.Remove(usuario);
                await _dbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
