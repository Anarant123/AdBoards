using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AdBoardsWebAPI.Models.db;

public partial class AdBoardsContext : DbContext
{
    public AdBoardsContext()
    {
    }

    public AdBoardsContext(DbContextOptions<AdBoardsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ad> Ads { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Right> Rights { get; set; }

    public virtual DbSet<TypeOfAd> TypeOfAds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=AdBoards;Trusted_Connection=True;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ad__3214EC0731877F8F");

            entity.ToTable("Ad");

            entity.Property(e => e.City).IsUnicode(false);
            entity.Property(e => e.CotegorysId).HasColumnName("Cotegorys_Id");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name).IsUnicode(false);
            entity.Property(e => e.PersonId).HasColumnName("Person_Id");
            entity.Property(e => e.TypeOfAdId).HasColumnName("TypeOfAd_Id");

            entity.HasOne(d => d.Cotegorys).WithMany(p => p.Ads)
                .HasForeignKey(d => d.CotegorysId)
                .HasConstraintName("FK__Ad__Cotegorys_Id__31EC6D26");

            entity.HasOne(d => d.Person).WithMany(p => p.Ads)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK__Ad__Person_Id__32E0915F");

            entity.HasOne(d => d.TypeOfAd).WithMany(p => p.Ads)
                .HasForeignKey(d => d.TypeOfAdId)
                .HasConstraintName("FK__Ad__TypeOfAd_Id__33D4B598");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07BB2336E2");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "UQ__Category__737584F668F2B573").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Complain__3214EC07AACC853A");

            entity.ToTable("Complaint");

            entity.Property(e => e.AdId).HasColumnName("Ad_Id");
            entity.Property(e => e.PersonId).HasColumnName("Person_Id");

            entity.HasOne(d => d.Ad).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.AdId)
                .HasConstraintName("FK__Complaint__Ad_Id__3A81B327");

            entity.HasOne(d => d.Person).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK__Complaint__Perso__3B75D760");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Favorite__3214EC07A5F6593D");

            entity.Property(e => e.AdId).HasColumnName("Ad_Id");
            entity.Property(e => e.PersonId).HasColumnName("Person_Id");

            entity.HasOne(d => d.Ad).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.AdId)
                .HasConstraintName("FK__Favorites__Ad_Id__36B12243");

            entity.HasOne(d => d.Person).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK__Favorites__Perso__37A5467C");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Person__3214EC07DFF65EE2");

            entity.ToTable("Person");

            entity.HasIndex(e => e.Login, "UQ__Person__5E55825B97E917E6").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Person__A9D10534A76CE9C4").IsUnique();

            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.City)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(13)
                .IsUnicode(false);
            entity.Property(e => e.RightId).HasColumnName("Right_Id");

            entity.HasOne(d => d.Right).WithMany(p => p.People)
                .HasForeignKey(d => d.RightId)
                .HasConstraintName("FK__Person__Right_Id__2F10007B");
        });

        modelBuilder.Entity<Right>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rights__3213E83FCA0A33F1");

            entity.HasIndex(e => e.Name, "UQ__Rights__737584F66547226E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TypeOfAd>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeOfAd__3214EC07D635DC85");

            entity.ToTable("TypeOfAd");

            entity.HasIndex(e => e.Name, "UQ__TypeOfAd__737584F6E73552B4").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
