using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace restaurante_web_app.Models;

public partial class Usuario
{
    public Guid IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    public string Contrasenia { get; set; } = null!;

    public short IdTipoUsuario { get; set; }
    [JsonIgnore]
    public virtual TiposUsuario IdTipoUsuarioNavigation { get; set; } = null!;
}
