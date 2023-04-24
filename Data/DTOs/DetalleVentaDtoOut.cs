namespace restaurante_web_app.Data.DTOs
{
    public class DetalleVentaDtoOut
    {
        public long Id { get; set; }
        public string? Platillo { get; set; } = null!;
        public decimal Precio { get; set; }
        public long? IdVenta { get; set; }
        public short? Cantidad { get; set; }
        public decimal? Subtotal { get; set; }
        public string? Observaciones { get; set; }
    }
}
