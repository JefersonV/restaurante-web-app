using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class CajaDiaria
{
    public long IdCajaDiaria { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal SaldoInicial { get; set; }

    public decimal? SaldoFinal { get; set; }

    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; } = new List<MovimientoCaja>();
}
