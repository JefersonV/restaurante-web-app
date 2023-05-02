using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class TiposUsuario
{
    public short IdTipoUsuario { get; set; }

    public string? Tipo { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
