using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace restaurante_web_app.Models;

public partial class GoeatContext : DbContext
{
    public GoeatContext()
    {
    }

    public GoeatContext(DbContextOptions<GoeatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CajaDiaria> CajaDiaria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVenta { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<GastosMovimientoCaja> GastosMovimientoCajas { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Mesero> Meseros { get; set; }

    public virtual DbSet<MovimientoCaja> MovimientoCajas { get; set; }

    public virtual DbSet<Proveedor> Proveedores { get; set; }

    public virtual DbSet<TipoMovimientoCaja> TipoMovimientoCajas { get; set; }

    public virtual DbSet<TiposUsuario> TiposUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<VentaMovimientoCaja> VentaMovimientoCajas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CajaDiaria>(entity =>
        {
            entity.HasKey(e => e.IdCajaDiaria).HasName("caja_diaria_pkey");

            entity.ToTable("caja_diaria");

            entity.Property(e => e.IdCajaDiaria).HasColumnName("id_caja_diaria");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.SaldoFinal)
                .HasColumnType("money")
                .HasColumnName("saldo_final");
            entity.Property(e => e.SaldoInicial)
                .HasColumnType("money")
                .HasColumnName("saldo_inicial");
            entity.Property(e => e.Estado)
            .HasColumnType("boolean")
            .HasColumnName("estado");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("clientes_pkey");

            entity.ToTable("clientes");

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.Institucion)
                .HasMaxLength(100)
                .HasColumnName("institucion");
            entity.Property(e => e.NombreApellido)
                .HasMaxLength(100)
                .HasColumnName("nombre_apellido");
            entity.Property(e => e.Puesto)
                .HasMaxLength(50)
                .HasColumnName("puesto");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.IdDetalleVenta).HasName("detalle_venta_pkey");

            entity.ToTable("detalle_venta");

            entity.Property(e => e.IdDetalleVenta).HasColumnName("id_detalle_venta");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdPlatillo).HasColumnName("id_platillo");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .HasColumnName("observaciones");
            entity.Property(e => e.Subtotal)
                .HasColumnType("money")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdPlatilloNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdPlatillo)
                .HasConstraintName("detalle_venta_id_platillo_fkey");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("detalle_venta_id_venta_fkey");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.IdGasto).HasName("gastos_pkey");

            entity.ToTable("gastos");

            entity.Property(e => e.IdGasto).HasColumnName("id_gasto");
            entity.Property(e => e.Concepto)
                .HasMaxLength(255)
                .HasColumnName("concepto");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(255)
                .HasColumnName("numero_documento");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Gastos)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("gastos_id_proveedor_fkey");
        });

        modelBuilder.Entity<GastosMovimientoCaja>(entity =>
        {
            entity.HasKey(e => e.IdGastoMovimientoCaja).HasName("gastos_movimiento_caja_pkey");

            entity.ToTable("gastos_movimiento_caja");

            entity.Property(e => e.IdGastoMovimientoCaja).HasColumnName("id_gasto_movimiento_caja");
            entity.Property(e => e.IdGasto).HasColumnName("id_gasto");
            entity.Property(e => e.IdMovimientoCaja).HasColumnName("id_movimiento_caja");

            entity.HasOne(d => d.IdGastoNavigation).WithMany(p => p.GastosMovimientoCajas)
                .HasForeignKey(d => d.IdGasto)
                .HasConstraintName("gastos_movimiento_caja_id_gasto_fkey");

            entity.HasOne(d => d.IdMovimientoCajaNavigation).WithMany(p => p.GastosMovimientoCajas)
                .HasForeignKey(d => d.IdMovimientoCaja)
                .HasConstraintName("gastos_movimiento_caja_id_movimiento_caja_fkey");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdPlatillo).HasName("menu_pkey");

            entity.ToTable("menu");

            entity.Property(e => e.IdPlatillo).HasColumnName("id_platillo");
            entity.Property(e => e.Platillo)
                .HasMaxLength(100)
                .HasColumnName("platillo");
            entity.Property(e => e.Precio)
                .HasColumnType("money")
                .HasColumnName("precio");
        });

        modelBuilder.Entity<Mesero>(entity =>
        {
            entity.HasKey(e => e.IdMesero).HasName("meseros_pkey");

            entity.ToTable("meseros");

            entity.Property(e => e.IdMesero).HasColumnName("id_mesero");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<MovimientoCaja>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("movimiento_caja_pkey");

            entity.ToTable("movimiento_caja");

            entity.Property(e => e.IdMovimiento).HasColumnName("id_movimiento");
            entity.Property(e => e.Concepto)
                .HasMaxLength(255)
                .HasColumnName("concepto");
            entity.Property(e => e.IdCajaDiaria).HasColumnName("id_caja_diaria");
            entity.Property(e => e.IdTipoMovimiento).HasColumnName("id_tipo_movimiento");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.IdCajaDiariaNavigation).WithMany(p => p.MovimientoCajas)
                .HasForeignKey(d => d.IdCajaDiaria)
                .HasConstraintName("movimiento_caja_id_caja_diaria_fkey");

            entity.HasOne(d => d.IdTipoMovimientoNavigation).WithMany(p => p.MovimientoCajas)
                .HasForeignKey(d => d.IdTipoMovimiento)
                .HasConstraintName("movimiento_caja_id_tipo_movimiento_fkey");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("proveedores_pkey");

            entity.ToTable("proveedores");

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(12)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<TipoMovimientoCaja>(entity =>
        {
            entity.HasKey(e => e.IdTipoMovimiento).HasName("tipo_movimiento_caja_pkey");

            entity.ToTable("tipo_movimiento_caja");

            entity.Property(e => e.IdTipoMovimiento).HasColumnName("id_tipo_movimiento");
            entity.Property(e => e.Tipo)
                .HasMaxLength(15)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<TiposUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuario).HasName("tipos_usuarios_pkey");

            entity.ToTable("tipos_usuarios");

            entity.Property(e => e.IdTipoUsuario).HasColumnName("id_tipo_usuario");
            entity.Property(e => e.Tipo)
                .HasMaxLength(30)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.Property(e => e.IdUsuario)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id_usuario");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(300)
                .HasColumnName("contraseña");
            entity.Property(e => e.IdTipoUsuario).HasColumnName("id_tipo_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .HasColumnName("usuario");

            entity.HasOne(d => d.IdTipoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdTipoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuarios_id_tipo_usuario_fkey");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.IdVenta).HasName("ventas_pkey");

            entity.ToTable("ventas");

            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdMesero).HasColumnName("id_mesero");
            entity.Property(e => e.NumeroComanda)
                .HasMaxLength(100)
                .HasColumnName("numero_comanda");
            entity.Property(e => e.Total)
                .HasColumnType("money")
                .HasColumnName("total");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("ventas_id_cliente_fkey");

            entity.HasOne(d => d.IdMeseroNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdMesero)
                .HasConstraintName("ventas_id_mesero_fkey");
        });

        modelBuilder.Entity<VentaMovimientoCaja>(entity =>
        {
            entity.HasKey(e => e.IdVentaMovimientoCaja).HasName("venta_movimiento_caja_pkey");

            entity.ToTable("venta_movimiento_caja");

            entity.Property(e => e.IdVentaMovimientoCaja).HasColumnName("id_venta_movimiento_caja");
            entity.Property(e => e.IdMovimientoCaja).HasColumnName("id_movimiento_caja");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");

            entity.HasOne(d => d.IdMovimientoCajaNavigation).WithMany(p => p.VentaMovimientoCajas)
                .HasForeignKey(d => d.IdMovimientoCaja)
                .HasConstraintName("venta_movimiento_caja_id_movimiento_caja_fkey");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.VentaMovimientoCajas)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("venta_movimiento_caja_id_venta_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
