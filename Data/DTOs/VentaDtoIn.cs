namespace restaurante_web_app.Data.DTOs
{
    public class VentaDtoIn
    {
            public long IdVenta { get; set; }
            public string? NumeroComanda { get; set; }
            public DateOnly? Fecha { get; set; }
            public decimal? Total { get; set; }
            public int? IdMesero { get; set; }
            public int? IdCliente { get; set; }
            public List<DetalleVentaDtoIn>? DetalleVenta { get; set; }

    }
}
