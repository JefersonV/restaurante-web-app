namespace restaurante_web_app.Data.DTOs
{
    public class InventarioDtoOut
    {
        public long IdMovimiento { get; set; }
        public string? TipoMovimiento { get; set; }
        public DateOnly FechaCaja { get; set; }
        public decimal? Total { get; set; }
    }
}
