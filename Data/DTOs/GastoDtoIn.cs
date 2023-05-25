namespace restaurante_web_app.Data.DTOs
{
    public class GastoDtoIn
    {
        public long IdGasto { get; set; }
        public string? NumeroDocumento { get; set; }

        public DateOnly? Fecha { get; set; }

        public string? Concepto { get; set; }

        public decimal? Total { get; set; }

        public int? IdProveedor { get; set; }
    }

}
