using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class GastosMovimientoCaja
{
    public long IdGastoMovimientoCaja { get; set; }

    public long? IdGasto { get; set; }

    public long? IdMovimientoCaja { get; set; }

    public virtual Gasto? IdGastoNavigation { get; set; }

    public virtual MovimientoCaja? IdMovimientoCajaNavigation { get; set; }
}
