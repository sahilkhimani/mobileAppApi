using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InoviDataAccessLayer.EntityModel
{
    public partial class QueryProContext : DbContext
    {
        public QueryProContext()
        {
        }

        public QueryProContext(DbContextOptions<QueryProContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAttachment> TblAttachments { get; set; } = null!;
        public virtual DbSet<TblQuery> TblQueries { get; set; } = null!;
        public virtual DbSet<TblQueryAttachment> TblQueryAttachments { get; set; } = null!;
        public virtual DbSet<TblQueryStatus> TblQueryStatuses { get; set; } = null!;
        public virtual DbSet<TblStatus> TblStatuses { get; set; } = null!;
        public virtual DbSet<TblUser> TblUsers { get; set; } = null!;
        public virtual DbSet<TblUserRole> TblUserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAttachment>(entity =>
            {
                entity.HasKey(e => e.AttachmentLinkId);

                entity.ToTable("tblAttachment");

                entity.Property(e => e.AttachmentLinkId).HasColumnName("attachment_link_id");

                entity.Property(e => e.CreatedById).HasColumnName("created_by_id");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.ModifiedById).HasColumnName("modified_by_id");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");

                entity.Property(e => e.Path).HasColumnName("path");

                entity.HasOne(d => d.PathNavigation)
                    .WithMany(p => p.TblAttachments)
                    .HasForeignKey(d => d.Path)
                    .HasConstraintName("FK_tblAttachment_tblQuery");
            });

            modelBuilder.Entity<TblQuery>(entity =>
            {
                entity.HasKey(e => e.QueryId)
                    .HasName("PK_Query");

                entity.ToTable("tblQuery");

                entity.Property(e => e.QueryId).HasColumnName("query_id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasMaxLength(10)
                    .HasColumnName("created_on")
                    .HasDefaultValueSql("(getdate())")
                    .IsFixedLength();

                entity.Property(e => e.CurrentStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("current_status");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on");

                entity.Property(e => e.Remarks).HasColumnName("remarks");

                entity.Property(e => e.RemarksBy).HasColumnName("remarks_by");

                entity.Property(e => e.RemarksOn)
                    .HasColumnType("datetime")
                    .HasColumnName("remarks_on");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblQueries)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Query_User");
            });

            modelBuilder.Entity<TblQueryAttachment>(entity =>
            {
                entity.HasKey(e => e.QueryAttachmentId);

                entity.ToTable("tblQueryAttachment");

                entity.Property(e => e.QueryAttachmentId).HasColumnName("query_attachment_id");

                entity.Property(e => e.AttachmentLinkId).HasColumnName("attachment_link_id");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.QueryId).HasColumnName("query_id");
            });

            modelBuilder.Entity<TblQueryStatus>(entity =>
            {
                entity.ToTable("tblQueryStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.QueryId).HasColumnName("query_id");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.HasOne(d => d.Query)
                    .WithMany(p => p.TblQueryStatuses)
                    .HasForeignKey(d => d.QueryId)
                    .HasConstraintName("FK_QueryStatus_Query");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.TblQueryStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_QueryStatus_Status");
            });

            modelBuilder.Entity<TblStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("PK_Status");

                entity.ToTable("tblStatus");

                entity.Property(e => e.StatusId).HasColumnName("status_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_User");

                entity.ToTable("tblUser");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreatedBy).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .HasColumnName("email_address");

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedBy).HasColumnName("modified_by");

                entity.Property(e => e.ModifiedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("modified_on")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.UserRoleId)
                    .HasConstraintName("FK_User_UserRole");
            });

            modelBuilder.Entity<TblUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId)
                    .HasName("PK_UserRole");

                entity.ToTable("tblUserRole");

                entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
