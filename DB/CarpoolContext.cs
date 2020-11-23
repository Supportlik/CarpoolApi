using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CarPoolApi.DB
{
    public partial class CarpoolContext : DbContext
    {
   

        public CarpoolContext(DbContextOptions<CarpoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Fahrgemeinschaft> Fahrgemeinschafts { get; set; }
        public virtual DbSet<FahrgemeinschaftMitglied> FahrgemeinschaftMitglieds { get; set; }
        public virtual DbSet<Fahrt> Fahrts { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=ConnectionStrings.CarPool", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fahrgemeinschaft>(entity =>
            {
                entity.ToTable("fahrgemeinschaft");

                entity.HasIndex(e => e.CreatorId, "fahrgemeinschaft_user_id_fk");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatorId).HasColumnName("creator");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasColumnName("name")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Fahrgemeinschafts)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("fahrgemeinschaft_user_id_fk");
            });

            modelBuilder.Entity<FahrgemeinschaftMitglied>(entity =>
            {
                entity.ToTable("fahrgemeinschaft_mitglied");

                entity.HasIndex(e => e.FahrgemeinschaftId, "fahrgemeinschaft_mitglieder_fahrgemeinschaft_id_fk");

                entity.HasIndex(e => e.UserId, "fahrgemeinschaft_mitglieder_user_id_fk");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FahrgemeinschaftId).HasColumnName("fahrgemeinschaft_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Fahrgemeinschaft)
                    .WithMany(p => p.FahrgemeinschaftMitglieds)
                    .HasForeignKey(d => d.FahrgemeinschaftId)
                    .HasConstraintName("fahrgemeinschaft_mitglieder_fahrgemeinschaft_id_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FahrgemeinschaftMitglieds)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fahrgemeinschaft_mitglieder_user_id_fk");
            });

            modelBuilder.Entity<Fahrt>(entity =>
            {
                entity.ToTable("fahrt");

                entity.HasIndex(e => e.FahrgemeinschaftId, "fahrt_fahrgemeinschaft_id_fk");

                entity.HasIndex(e => e.FahrerId, "fahrt_fahrgemeinschaft_mitglieder_id_fk");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.FahrerId).HasColumnName("fahrer_id");

                entity.Property(e => e.FahrgemeinschaftId).HasColumnName("fahrgemeinschaft_id");

                entity.HasOne(d => d.Fahrer)
                    .WithMany(p => p.Fahrts)
                    .HasForeignKey(d => d.FahrerId)
                    .HasConstraintName("fahrt_fahrgemeinschaft_mitglieder_id_fk");

                entity.HasOne(d => d.Fahrgemeinschaft)
                    .WithMany(p => p.Fahrts)
                    .HasForeignKey(d => d.FahrgemeinschaftId)
                    .HasConstraintName("fahrt_fahrgemeinschaft_id_fk");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.OauthId, "user_oauth_id_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nachname)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasColumnName("nachname")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.OauthId)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasColumnName("oauth_id")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Vorname)
                    .IsRequired()
                    .HasColumnType("varchar(255)")
                    .HasColumnName("vorname")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
