namespace restaurante_web_app.Data.DTOs
{
    public class DetalleVentaDtoIn
    {
        public long IdDetalleVenta { get; set; }
        public int? IdPlatillo { get; set; }
        public long? IdVenta { get; set; }
        public short? Cantidad { get; set; }
        public decimal? Subtotal { get; set; }
        public string? Observaciones { get; set; }
    }

}
