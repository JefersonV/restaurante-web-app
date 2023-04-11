using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public virtual ICollection<Gasto> Gastos { get; } = new List<Gasto>();
}
