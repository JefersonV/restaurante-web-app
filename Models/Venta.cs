using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

    [JsonIgnore]
    public virtual Cliente? IdClienteNavigation { get; set; }
    [JsonIgnore]
    public virtual Mesero? IdMeseroNavigation { get; set; }
}
