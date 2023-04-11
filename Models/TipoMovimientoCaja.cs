using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class TipoMovimientoCaja
{
    public short IdTipoMovimiento { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; } = new List<MovimientoCaja>();
}
