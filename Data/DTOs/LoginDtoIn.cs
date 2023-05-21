using System.ComponentModel.DataAnnotations;

namespace restaurante_web_app.Data.DTOs
{
    public class LoginDtoIn
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string Usuario { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contrasenia { get; set; } = null!;
    }
}
