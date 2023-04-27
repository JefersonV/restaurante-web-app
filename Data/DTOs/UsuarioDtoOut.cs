namespace restaurante_web_app.Data.DTOs
{
    public class UsuarioDtoOut
    {
        public Guid IdUsuario { get; set; }
        public string Nombre { get; set; } = null!;

        public string Usuario1 { get; set; } = null!;

        public string TipoUsuario { get; set; } = null!;
    }
}
