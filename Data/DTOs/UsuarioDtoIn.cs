namespace restaurante_web_app.Data.DTOs
{
    public class UsuarioDtoIn
    {
        public Guid IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Usuario1 { get; set; }

        public string Contrasenia { get; set; } = null!;

        public short IdTipoUsuario { get; set; }
    }
}
