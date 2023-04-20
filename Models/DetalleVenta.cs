using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace restaurante_web_app.Models;

public partial class DetalleVenta
{
    public long IdDetalleVenta { get; set; }

    public int? IdPlatillo { get; set; }

    public long? IdVenta { get; set; }

    public short? Cantidad { get; set; }

    public decimal? Subtotal { get; set; }

    public string? Observaciones { get; set; }
    [JsonIgnore]
    public virtual Menu? IdPlatilloNavigation { get; set; }
    [JsonIgnore]
    public virtual Venta? IdVentaNavigation { get; set; }
}
