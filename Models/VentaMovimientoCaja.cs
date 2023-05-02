using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class VentaMovimientoCaja
{
    public long IdVentaMovimientoCaja { get; set; }

    public long? IdVenta { get; set; }

    public long? IdMovimientoCaja { get; set; }

    public virtual MovimientoCaja? IdMovimientoCajaNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}
