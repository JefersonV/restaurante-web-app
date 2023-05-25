using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace restaurante_web_app.Models;

public partial class CajaDiaria
{
    public long IdCajaDiaria { get; set; }

    public DateOnly Fecha { get; set; }

    public decimal? SaldoInicial { get; set; }

    public decimal? SaldoFinal { get; set; }
    public Boolean Estado { get; set; }
    [JsonIgnore]
    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; } = new List<MovimientoCaja>();
}
