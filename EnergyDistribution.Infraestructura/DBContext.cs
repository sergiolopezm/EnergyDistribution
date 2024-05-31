using Microsoft.EntityFrameworkCore;

namespace EnergyDistribution.Infraestructure
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Acceso> Accesos { get; set; } = null!;
        public virtual DbSet<Consumo> Consumos { get; set; } = null!;
        public virtual DbSet<Costo> Costos { get; set; } = null!;
        public virtual DbSet<Log> Logs { get; set; } = null!;
        public virtual DbSet<Perdidum> Perdida { get; set; } = null!;
        public virtual DbSet<Token> Tokens { get; set; } = null!;
        public virtual DbSet<TokenExpirado> TokenExpirados { get; set; } = null!;
        public virtual DbSet<Tramo> Tramos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Acceso>(entity =>
            {
                entity.HasKey(e => e.IdAcceso)
                    .HasName("PK__Acceso__99B2858F8985A006");

                entity.ToTable("Acceso");

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Sitio)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Consumo>(entity =>
            {
                entity.ToTable("Consumo");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.HasOne(d => d.Tramo)
                    .WithMany(p => p.Consumos)
                    .HasForeignKey(d => d.TramoId)
                    .HasConstraintName("FK__Consumo__TramoId__38996AB5");
            });

            modelBuilder.Entity<Costo>(entity =>
            {
                entity.ToTable("Costo");

                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.HasOne(d => d.Tramo)
                    .WithMany(p => p.Costos)
                    .HasForeignKey(d => d.TramoId)
                    .HasConstraintName("FK__Costo__TramoId__3B75D760");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.IdLog)
                    .HasName("PK__Log__0C54DBC6BC9AB237");

                entity.ToTable("Log");

                entity.Property(e => e.Accion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Detalle)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ip)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Tipo)
                    .HasMaxLength(3)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Perdidum>(entity =>
            {
                entity.Property(e => e.Fecha).HasColumnType("date");

                entity.HasOne(d => d.Tramo)
                    .WithMany(p => p.Perdida)
                    .HasForeignKey(d => d.TramoId)
                    .HasConstraintName("FK__Perdida__TramoId__3E52440B");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(e => e.IdToken)
                    .HasName("PK__Token__D6332447369DCBEB");

                entity.ToTable("Token");

                entity.Property(e => e.IdToken)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAutenticacion).HasColumnType("datetime");

                entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ip)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Tokens)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Token.IdUsuario");
            });

            modelBuilder.Entity<TokenExpirado>(entity =>
            {
                entity.HasKey(e => e.IdToken)
                    .HasName("PK__TokenExp__D63324474ABB5AFF");

                entity.ToTable("TokenExpirado");

                entity.Property(e => e.IdToken)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAutenticacion).HasColumnType("datetime");

                entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ip)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TokenExpirados)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_TokenExpirado.IdUsuario");
            });

            modelBuilder.Entity<Tramo>(entity =>
            {
                entity.ToTable("Tramo");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__Usuario__5B65BF97F75ABB26");

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Apellido)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Contraseña)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuario)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Rol)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });
        }
    }
}
