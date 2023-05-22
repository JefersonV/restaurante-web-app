namespace restaurante_web_app.Data.DTOs
{
    public class CajaDtoOut
    {
        public long IdCajaDiaria { get; set; }
        public DateOnly Fecha { get; set; }
        public Boolean Estado { get; set; }
        public decimal? SaldoInicial { get; set; }
        public decimal? Ingreso { get; set; }
        public decimal? Egreso { get; set; }
        public decimal? Caja { get; set; }
        public decimal? Entrega { get; set; }
        public decimal? SaldoBruto { get; set; }
        public decimal? Ganancia { get; set; }

    }
}
