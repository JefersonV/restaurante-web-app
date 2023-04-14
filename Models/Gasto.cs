using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class Gasto
{
    public long IdGasto { get; set; }

    public string? NumeroDocumento { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? Concepto { get; set; }

    public decimal? Total { get; set; }

    public int? IdProveedor { get; set; }

    public virtual Proveedor? IdProveedorNavigation { get; set; }
}
