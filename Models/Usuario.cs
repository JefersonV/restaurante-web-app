﻿using System;
using System.Collections.Generic;

namespace restaurante_web_app.Models;

public partial class Usuario
{
    public Guid IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public short IdTipoUsuario { get; set; }

    public virtual TiposUsuario IdTipoUsuarioNavigation { get; set; } = null!;
}
