using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace restaurante_web_app.Models;

public partial class Menu
{
    public int IdPlatillo { get; set; }

    public string Platillo { get; set; } = null!;

    public decimal Precio { get; set; }
    [JsonIgnore]
    public virtual ICollection<DetalleVenta> DetalleVenta { get; } = new List<DetalleVenta>();
}
