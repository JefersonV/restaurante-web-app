using restaurante_web_app.Models;

namespace restaurante_web_app.Data.DTOs
{

    public class CajaDiariaDTO
    {
        public long IdCajaDiaria { get; set; }
        public DateOnly Fecha { get; set; }
        public decimal? SaldoInicial { get; set; }
        public decimal? SaldoFinal { get; set; }
        public bool Estado { get; set; }
        public List<MovimientoCajaDTO> MovimientosCaja { get; set; }
    }

    public class MovimientoCajaDTO
    {
        public long IdMovimiento { get; set; }
        public TipoMovimientoCajaDTO TipoMovimiento { get; set; }
        public string Concepto { get; set; }
        public decimal? Total { get; set; }
        public List<GastoDTO> Gastos { get; set; }
        public List<VentaDTO> Ventas { get; set; }
    }

    public class GastoDTO
    {
        public long IdGasto { get; set; }
        public string NumeroDocumento { get; set; }
        public DateOnly? Fecha { get; set; }
        public string Concepto { get; set; }
        public decimal? Total { get; set; }
        public ProveedorDTO Proveedor { get; set; }
    }

    public class VentaDTO
    {
        public long IdVenta { get; set; }
        public string NumeroComanda { get; set; }
        public DateOnly? Fecha { get; set; }
        public decimal? Total { get; set; }
        public int? IdMesero { get; set; }
        public int? IdCliente { get; set; }
        public List<DetalleVentaDTO> DetalleVenta { get; set; }
    }

    public class DetalleVentaDTO
    {
        public int IdPlatillo { get; set; }
        public string Platillo { get; set; }
        public decimal Precio { get; set; }
    }

    public class ProveedorDTO
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }

        // Otros atributos del proveedor
    }

    public class TipoMovimientoCajaDTO
    {
        public short IdTipoMovimiento { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<MovimientoCaja> MovimientoCajas { get; } = new List<MovimientoCaja>();

        // Otros atributos del tipo de movimiento
    }



}