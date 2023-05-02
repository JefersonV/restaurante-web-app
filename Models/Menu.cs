using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class Menu
{
    public int IdPlatillo { get; set; }

    public string Platillo { get; set; } = null!;

    public decimal Precio { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; } = new List<DetalleVenta>();
}
