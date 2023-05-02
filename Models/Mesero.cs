using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class Mesero
{
    public int IdMesero { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; } = new List<Venta>();
}
