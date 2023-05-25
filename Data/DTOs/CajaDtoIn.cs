namespace restaurante_web_app.Data.DTOs
{
    public class CajaDtoIn
    {
        public long IdCajaDiaria { get; set; }

        public DateOnly Fecha { get; set; }

        public decimal? SaldoInicial { get; set; }
    }
}
