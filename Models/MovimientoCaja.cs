using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class MovimientoCaja
{
    public long IdMovimiento { get; set; }

    public short? IdTipoMovimiento { get; set; }

    public long? IdCajaDiaria { get; set; }

    public string? Concepto { get; set; }

    public decimal? Total { get; set; }

    public virtual ICollection<GastosMovimientoCaja> GastosMovimientoCajas { get; } = new List<GastosMovimientoCaja>();

    public virtual CajaDiaria? IdCajaDiariaNavigation { get; set; }

    public virtual TipoMovimientoCaja? IdTipoMovimientoNavigation { get; set; }

    public virtual ICollection<VentaMovimientoCaja> VentaMovimientoCajas { get; } = new List<VentaMovimientoCaja>();
}
