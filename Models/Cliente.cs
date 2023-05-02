using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string NombreApellido { get; set; } = null!;

    public DateOnly? Fecha { get; set; }

    public string? Institucion { get; set; }

    public string? Puesto { get; set; }

    public virtual ICollection<Venta> Venta { get; } = new List<Venta>();
}
