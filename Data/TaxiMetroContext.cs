using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Threading;
using TaxiMetro.Model;
using TaxiMetro.Model.Common;

namespace TaxiMetro.DataAccess
{
    public class TaxiMetroContext : DbContext
    {
        public class MigrationsContextFactory : IDbContextFactory<TaxiMetroContext>
        {
            public TaxiMetroContext Create()
            {
                return new TaxiMetroContext();
            }
        }

        #region Propiedades

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Nivel> Nivel { get; set; }
        public DbSet<Taxi> Taxi { get; set; }
        public DbSet<Turno> Turno { get; set; }
        public DbSet<Documento> Documento { get; set; }
        public DbSet<Hoja> Hoja { get; set; }
        public DbSet<TipoGasto> TipoGasto { get; set; }
        public DbSet<Gasto> Gasto { get; set; }
        public DbSet<Aviso> Aviso { get; set; }
        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<Tarjeta> Tarjeta { get; set; }
        public DbSet<TipoTarjeta> TipoTarjeta { get; set; }
        public DbSet<PartidaTarjetas> PartidaTarjetas { get; set; }

        #endregion

        #region Constructor

        public TaxiMetroContext(bool create = true)
            : base("TaxiMetroData_db")
        {
            //if (create)
            //    Database.SetInitializer<TaxiMetroContext>(new TaxiMetroInitializer());
            //else
            //{
            //    Database.SetInitializer<TaxiMetroContext>(new DropCreateDatabaseAlways<TaxiMetroContext>());
            //}
        }

        #endregion

        #region Métodos

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new NivelMap());
            modelBuilder.Configurations.Add(new TaxiMap());
            modelBuilder.Configurations.Add(new TurnoMap());
            modelBuilder.Configurations.Add(new DocumentoMap());
            modelBuilder.Configurations.Add(new HojaMap());
            modelBuilder.Configurations.Add(new TipoGastoMap());
            modelBuilder.Configurations.Add(new GastoMap());
            modelBuilder.Configurations.Add(new AvisoMap());
            modelBuilder.Configurations.Add(new TareaMap());
            modelBuilder.Configurations.Add(new TarjetaMap());
            modelBuilder.Configurations.Add(new TipoTarjetaMap());
            modelBuilder.Configurations.Add(new PartidaTarjetasMap());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity
                    && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    var identityName = Thread.CurrentPrincipal.Identity.Name;
                    var now = DateTime.UtcNow;

                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreatedDate = now;
                    }
                    else
                    {
                        Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                    }

                    entity.UpdatedBy = identityName;
                    entity.UpdatedDate = now;
                }
            }

            return base.SaveChanges();
        }

        #endregion

        #region Configuración de las Tablas

        public class UsuarioMap : EntityTypeConfiguration<Usuario>
        {
            public UsuarioMap()
            {
                ToTable("Usuario");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
               
                HasMany<Taxi>(x => x.Taxis).WithRequired(x => x.Usuario).HasForeignKey(x => x.UsuarioId).WillCascadeOnDelete(false);
                HasMany<Turno>(x => x.Turnos).WithRequired(x => x.Usuario).HasForeignKey(x => x.UsuarioId).WillCascadeOnDelete(false);
                HasMany<Documento>(x => x.Documentos).WithRequired(x => x.Usuario).HasForeignKey(x => x.UsuarioId).WillCascadeOnDelete(false);
                HasMany<Hoja>(x => x.Hojas).WithRequired(x => x.Usuario).HasForeignKey(x => x.UsuarioId).WillCascadeOnDelete(false);
                HasMany<Hoja>(x => x.Hojas).WithRequired(x => x.Conductor).HasForeignKey(x => x.ConductorId).WillCascadeOnDelete(false);
                HasMany<Gasto>(x => x.Gastos).WithRequired(x => x.Usuario).HasForeignKey(x => x.UsuarioId).WillCascadeOnDelete(false);
                HasMany<Tarea>(x => x.Tareas).WithRequired(x => x.Usuario).HasForeignKey(x => x.UsuarioId).WillCascadeOnDelete(false);
                HasMany<Tarjeta>(x => x.Tarjetas).WithRequired(x => x.Usuario).HasForeignKey(x => x.UsuarioId).WillCascadeOnDelete(false);
                HasMany<PartidaTarjetas>(x => x.PartidasTarjetas).WithRequired(x => x.Usuario).HasForeignKey(x => x.UsuarioId).WillCascadeOnDelete(false);

                HasOptional(x => x.UsuarioPadre).WithMany(x => x.UsuarioHijos).HasForeignKey(x => x.UsuarioId);

                Property(c => c.NivelId).IsRequired();
                Property(c => c.UsuarioId).IsOptional();
                Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            }
        }

        public class NivelMap : EntityTypeConfiguration<Nivel>
        {
            public NivelMap()
            {
                ToTable("Nivel");

                HasKey(c => c.Id);
                
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                HasMany<Usuario>(x => x.Usuarios).WithRequired(x => x.Nivel).HasForeignKey(x => x.NivelId);

                Property(c => c.Descripcion).IsRequired().HasMaxLength(100);
            }
        }

        public class TaxiMap : EntityTypeConfiguration<Taxi>
        {
            public TaxiMap()
            {
                ToTable("Taxi");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                HasMany<Hoja>(x => x.Hojas).WithRequired(x => x.Taxi).HasForeignKey(x => x.TaxiId);
                HasMany<Aviso>(x => x.Avisos).WithRequired(x => x.Taxi).HasForeignKey(x => x.TaxiId);
                HasMany<Tarjeta>(x => x.Tarjetas).WithRequired(x => x.Taxi).HasForeignKey(x => x.TaxiId).WillCascadeOnDelete(false);

                Property(c => c.Matricula).IsRequired().HasMaxLength(20);
            }
        }

        public class TurnoMap : EntityTypeConfiguration<Turno>
        {
            public TurnoMap()
            {
                ToTable("Turno");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                Property(c => c.Descripcion).IsRequired().HasMaxLength(50);
            }
        }

        public class DocumentoMap : EntityTypeConfiguration<Documento>
        {
            public DocumentoMap()
            {
                ToTable("Documento");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                Property(c => c.Descripcion).IsRequired().HasMaxLength(25);
            }
        }

        public class HojaMap : EntityTypeConfiguration<Hoja>
        {
            public HojaMap()
            {
                ToTable("Hoja");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(c => c.UsuarioId).IsRequired();
                Property(c => c.ConductorId).IsRequired();
                Property(c => c.TaxiId).IsRequired();
                Property(c => c.TurnoId).IsRequired();
                Property(c => c.Observaciones).HasMaxLength(200);

                HasMany<Tarjeta>(x => x.Tarjetas).WithRequired(x => x.Hoja).HasForeignKey(x => x.HojaId).WillCascadeOnDelete(false);
            }
        }

        public class TipoGastoMap : EntityTypeConfiguration<TipoGasto>
        {
            public TipoGastoMap()
            {
                ToTable("TipoGasto");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                Property(c => c.Descripcion).IsRequired().HasMaxLength(50);

                HasMany<Gasto>(x => x.Gastos).WithRequired(x => x.TipoGasto).HasForeignKey(x => x.TipoGastoId).WillCascadeOnDelete(false);
            }
        }

        public class GastoMap : EntityTypeConfiguration<Gasto>
        {
            public GastoMap()
            {
                ToTable("Gasto");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                Property(c => c.UsuarioId).IsRequired();
                Property(c => c.TipoGastoId).IsRequired();
                Property(c => c.Fecha).IsRequired();
                Property(c => c.Importe).IsRequired();               

                HasOptional(g => g.Taxi).WithMany(t => t.Gastos).HasForeignKey(g => g.TaxiId);
                HasMany<Tarea>(x => x.Tareas).WithOptional(x => x.Gasto).HasForeignKey(x => x.GastoId).WillCascadeOnDelete(false);
            }
        }

        public class AvisoMap : EntityTypeConfiguration<Aviso>
        {
            public AvisoMap()
            {
                ToTable("Aviso");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(c => c.TaxiId).IsRequired();
                Property(c => c.Descripcion).HasMaxLength(200);
            }
        }


        public class TareaMap : EntityTypeConfiguration<Tarea>
        {
            public TareaMap()
            {
                ToTable("Tarea");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(c => c.UsuarioId).IsRequired();
                Property(c => c.Descripcion).HasMaxLength(200);

                HasOptional(g => g.Taxi).WithMany(t => t.Tareas).HasForeignKey(g => g.TaxiId);
//                HasOptional(g => g.Gasto).WithMany(t => t.Tareas).HasForeignKey(g => g.GastoId);
            }
        }

        public class TipoTarjetaMap : EntityTypeConfiguration<TipoTarjeta>
        {
            public TipoTarjetaMap()
            {
                ToTable("TipoTarjeta");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                Property(c => c.Descripcion).IsRequired().HasMaxLength(50);

                HasMany<Tarjeta>(x => x.Tarjetas).WithRequired(x => x.TipoTarjeta).HasForeignKey(x => x.TipoTarjetaId).WillCascadeOnDelete(false);
            }
        }

        public class TarjetaMap : EntityTypeConfiguration<Tarjeta>
        {
            public TarjetaMap()
            {
                ToTable("Tarjeta");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(c => c.UsuarioId).IsRequired();
                Property(c => c.TipoTarjetaId).IsRequired();
                Property(c => c.TaxiId).IsRequired();
                Property(c => c.Fecha).IsRequired();
                Property(c => c.Importe).IsRequired();
            }
        }

        public class PartidaTarjetasMap : EntityTypeConfiguration<PartidaTarjetas>
        {
            public PartidaTarjetasMap()
            {
                ToTable("PartidaTarjetas");

                HasKey(c => c.Id);
                Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
                Property(c => c.UsuarioId).IsRequired();
                Property(c => c.Descripcion).HasMaxLength(200);

                HasMany<Tarjeta>(x => x.Tarjetas).WithOptional(x => x.PartidaTarjetas).HasForeignKey(x => x.PartidaTarjetasId).WillCascadeOnDelete(false);
                HasOptional(pt => pt.Gasto).WithOptionalDependent(g => g.PartidaTarjetas).Map(p => p.MapKey("PartidaTarjetasId")); // Funciona mas o menos ¿? hay que poner PartidaTarjetasId
                //HasOptional(pt => pt.Gasto).WithMany().HasForeignKey(g => g.GastoId); // No funciona correctamente ¿?
            }
        }

        #endregion
    }
}
