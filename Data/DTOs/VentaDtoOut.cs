namespace restaurante_web_app.Data.DTOs
{
    public class VentaDtoOut
    {
        public long IdVenta { get; set; }
        public string? NumeroComanda { get; set; }
        public DateOnly? Fecha { get; set; }
        public decimal? Total { get; set; }
        public string? Mesero { get; set; } = null!;
        public string? Cliente { get; set; } = null!;
        public List<DetalleVentaDtoOut>? DetalleVenta { get; set; }
    }
}
