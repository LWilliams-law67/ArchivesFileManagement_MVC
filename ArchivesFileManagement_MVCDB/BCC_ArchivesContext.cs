﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ArchivesFileManagement_MVCDB
{
    public partial class BCC_ArchivesContext : DbContext
    {
        public BCC_ArchivesContext()
        {
        }

        public BCC_ArchivesContext(DbContextOptions<BCC_ArchivesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Hccccasetypes> Hccccasetypes { get; set; }
        public virtual DbSet<Hcccmain> Hcccmain { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SpecialCollections> SpecialCollections { get; set; }
        public virtual DbSet<SpecialCollectionsTypes> SpecialCollectionsTypes { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=BCCAPPSDEV\\DEV2014;Initial Catalog=BCC_Archives;Persist Security Info=True;User ID=BCC_ArchivesUser;Password=BCC_ArchivesPass");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Hccccasetypes>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);
            });

            modelBuilder.Entity<Hcccmain>(entity =>
            {
                entity.Property(e => e.CaseNo).IsUnicode(false);

                entity.Property(e => e.CreationDate).HasComputedColumnSql("(getdate())", false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.DestructionDate).IsUnicode(false);

                entity.Property(e => e.EditedBy).IsUnicode(false);

                entity.Property(e => e.Location).IsUnicode(false);

                entity.Property(e => e.SessionNo).IsUnicode(false);

                entity.Property(e => e.UploadedBy).IsUnicode(false);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Hcccmain)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HCCCMAIN_HCCCCASETYPES");
            });

            modelBuilder.Entity<SpecialCollections>(entity =>
            {
                entity.HasOne(d => d.Type)
                    .WithMany(p => p.SpecialCollections)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecialCollections_SpecialCollectionsTypes");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}