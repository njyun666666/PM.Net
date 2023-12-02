using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PMDB.Models;

public partial class PmdbContext : DbContext
{
    public PmdbContext(DbContextOptions<PmdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbMenu> TbMenus { get; set; }

    public virtual DbSet<TbOrgDept> TbOrgDepts { get; set; }

    public virtual DbSet<TbOrgDeptUser> TbOrgDeptUsers { get; set; }

    public virtual DbSet<TbOrgRole> TbOrgRoles { get; set; }

    public virtual DbSet<TbOrgRoleUser> TbOrgRoleUsers { get; set; }

    public virtual DbSet<TbOrgUser> TbOrgUsers { get; set; }

    public virtual DbSet<TbRefreshToken> TbRefreshTokens { get; set; }

    public virtual DbSet<VwOrgCompany> VwOrgCompanies { get; set; }

    public virtual DbSet<VwOrgDept> VwOrgDepts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<TbMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PRIMARY");

            entity.ToTable("TbMenu");

            entity.Property(e => e.MenuId)
                .HasMaxLength(50)
                .HasColumnName("MenuID");
            entity.Property(e => e.Icon).HasMaxLength(50);
            entity.Property(e => e.MenuName).HasMaxLength(50);
            entity.Property(e => e.ParentMenuId)
                .HasMaxLength(50)
                .HasColumnName("ParentMenuID");
            entity.Property(e => e.Url)
                .HasMaxLength(50)
                .HasColumnName("URL");
        });

        modelBuilder.Entity<TbOrgDept>(entity =>
        {
            entity.HasKey(e => e.Did).HasName("PRIMARY");

            entity.ToTable("TbOrgDept");

            entity.HasIndex(e => e.ParentDid, "idx_ParentDID");

            entity.HasIndex(e => new { e.RootDid, e.Did }, "idx_RootDID_DID");

            entity.Property(e => e.Did)
                .HasMaxLength(50)
                .HasColumnName("DID");
            entity.Property(e => e.DeptName).HasMaxLength(50);
            entity.Property(e => e.ParentDid)
                .HasMaxLength(50)
                .HasColumnName("ParentDID");
            entity.Property(e => e.RootDid)
                .HasMaxLength(50)
                .HasColumnName("RootDID");
        });

        modelBuilder.Entity<TbOrgDeptUser>(entity =>
        {
            entity.HasKey(e => new { e.Did, e.Uid })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("TbOrgDeptUser");

            entity.HasIndex(e => e.Uid, "UID");

            entity.Property(e => e.Did)
                .HasMaxLength(50)
                .HasColumnName("DID");
            entity.Property(e => e.Uid)
                .HasMaxLength(50)
                .HasColumnName("UID");

            entity.HasOne(d => d.DidNavigation).WithMany(p => p.TbOrgDeptUsers)
                .HasForeignKey(d => d.Did)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TbOrgDeptUser_ibfk_1");

            entity.HasOne(d => d.UidNavigation).WithMany(p => p.TbOrgDeptUsers)
                .HasForeignKey(d => d.Uid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TbOrgDeptUser_ibfk_2");
        });

        modelBuilder.Entity<TbOrgRole>(entity =>
        {
            entity.HasKey(e => e.Rid).HasName("PRIMARY");

            entity.ToTable("TbOrgRole");

            entity.Property(e => e.Rid)
                .HasMaxLength(50)
                .HasColumnName("RID");
            entity.Property(e => e.RoleName).HasMaxLength(50);

            entity.HasMany(d => d.Menus).WithMany(p => p.Rids)
                .UsingEntity<Dictionary<string, object>>(
                    "TbAuth",
                    r => r.HasOne<TbMenu>().WithMany()
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TbAuth_ibfk_2"),
                    l => l.HasOne<TbOrgRole>().WithMany()
                        .HasForeignKey("Rid")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("TbAuth_ibfk_1"),
                    j =>
                    {
                        j.HasKey("Rid", "MenuId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("TbAuth");
                        j.HasIndex(new[] { "MenuId" }, "MenuID");
                        j.IndexerProperty<string>("Rid")
                            .HasMaxLength(50)
                            .HasColumnName("RID");
                        j.IndexerProperty<string>("MenuId")
                            .HasMaxLength(50)
                            .HasColumnName("MenuID");
                    });
        });

        modelBuilder.Entity<TbOrgRoleUser>(entity =>
        {
            entity.HasKey(e => new { e.Uid, e.Rid, e.RootDid })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("TbOrgRoleUser");

            entity.HasIndex(e => e.Rid, "RID");

            entity.HasIndex(e => e.RootDid, "TbOrgRoleUser_ibfk_3_idx");

            entity.Property(e => e.Uid)
                .HasMaxLength(50)
                .HasColumnName("UID");
            entity.Property(e => e.Rid)
                .HasMaxLength(50)
                .HasColumnName("RID");
            entity.Property(e => e.RootDid)
                .HasMaxLength(50)
                .HasColumnName("RootDID");

            entity.HasOne(d => d.RidNavigation).WithMany(p => p.TbOrgRoleUsers)
                .HasForeignKey(d => d.Rid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TbOrgRoleUser_ibfk_2");

            entity.HasOne(d => d.RootD).WithMany(p => p.TbOrgRoleUsers)
                .HasForeignKey(d => d.RootDid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TbOrgRoleUser_ibfk_3");

            entity.HasOne(d => d.UidNavigation).WithMany(p => p.TbOrgRoleUsers)
                .HasForeignKey(d => d.Uid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TbOrgRoleUser_ibfk_1");
        });

        modelBuilder.Entity<TbOrgUser>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("PRIMARY");

            entity.ToTable("TbOrgUser");

            entity.HasIndex(e => e.Email, "EMail").IsUnique();

            entity.Property(e => e.Uid)
                .HasMaxLength(50)
                .HasColumnName("UID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("EMail");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.OauthProvider)
                .HasMaxLength(50)
                .HasColumnName("OAuthProvider");
            entity.Property(e => e.Passwrod).HasMaxLength(255);
            entity.Property(e => e.PhotoUrl).HasMaxLength(100);
        });

        modelBuilder.Entity<TbRefreshToken>(entity =>
        {
            entity.HasKey(e => e.RefreshToken).HasName("PRIMARY");

            entity.ToTable("TbRefreshToken");

            entity.Property(e => e.ExpireTime).HasColumnType("datetime");
            entity.Property(e => e.Uid)
                .HasMaxLength(255)
                .HasColumnName("UID");
        });

        modelBuilder.Entity<VwOrgCompany>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_OrgCompany");

            entity.Property(e => e.DeptName).HasMaxLength(50);
            entity.Property(e => e.Did)
                .HasMaxLength(50)
                .HasColumnName("DID");
            entity.Property(e => e.ParentDid)
                .HasMaxLength(50)
                .HasColumnName("ParentDID");
            entity.Property(e => e.RootDid)
                .HasMaxLength(50)
                .HasColumnName("RootDID");
        });

        modelBuilder.Entity<VwOrgDept>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_OrgDept");

            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.DeptName).HasMaxLength(50);
            entity.Property(e => e.Did)
                .HasMaxLength(50)
                .HasColumnName("DID");
            entity.Property(e => e.ParentDeptName).HasMaxLength(50);
            entity.Property(e => e.ParentDid)
                .HasMaxLength(50)
                .HasColumnName("ParentDID");
            entity.Property(e => e.RootDid)
                .HasMaxLength(50)
                .HasColumnName("RootDID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
