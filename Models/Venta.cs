using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class Venta
{
    public long IdVenta { get; set; }

    public string? NumeroComanda { get; set; }

    public DateOnly? Fecha { get; set; }

    public decimal? Total { get; set; }

    public int? IdMesero { get; set; }

    public int? IdCliente { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; } = new List<DetalleVenta>();

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Mesero? IdMeseroNavigation { get; set; }

    public virtual ICollection<VentaMovimientoCaja> VentaMovimientoCajas { get; } = new List<VentaMovimientoCaja>();
}
