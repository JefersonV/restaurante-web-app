using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace restaurante_web_app.Models;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    [JsonIgnore]
    public virtual ICollection<Gasto> Gastos { get; } = new List<Gasto>();
}