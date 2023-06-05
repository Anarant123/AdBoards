#nullable disable

using AdBoardsWebAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AdBoardsWebAPI.Data;

public class AdBoardsContext : DbContext
{
    public AdBoardsContext()
    {
    }

    public AdBoardsContext(DbContextOptions<AdBoardsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ad> Ads { get; set; }

    public virtual DbSet<AdType> AdTypes { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Right> Rights { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Name=ConnectionStrings:AdBoards");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ad_pkey");

            entity.ToTable("ad");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdTypeId).HasColumnName("ad_type_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.City)
                .HasMaxLength(64)
                .HasColumnName("city");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.PhotoName)
                .HasMaxLength(64)
                .HasColumnName("photo_name");
            entity.Property(e => e.Price).HasColumnName("price");

            entity.HasOne(d => d.AdType).WithMany(p => p.Ads)
                .HasForeignKey(d => d.AdTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ad_ad_type_id_fkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Ads)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ad_category_id_fkey");

            entity.HasOne(d => d.Person).WithMany(p => p.Ads)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ad_person_id_fkey");
        });

        modelBuilder.Entity<AdType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ad_type_pkey");

            entity.ToTable("ad_type");

            entity.HasIndex(e => e.Name, "ad_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.ToTable("category");

            entity.HasIndex(e => e.Name, "category_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("complaint_pkey");

            entity.ToTable("complaint");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdId).HasColumnName("ad_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");

            entity.HasOne(d => d.Ad).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.AdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("complaint_ad_id_fkey");

            entity.HasOne(d => d.Person).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("complaint_person_id_fkey");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("favorites_pkey");

            entity.ToTable("favorites");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdId).HasColumnName("ad_id");
            entity.Property(e => e.PersonId).HasColumnName("person_id");

            entity.HasOne(d => d.Ad).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.AdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favorites_ad_id_fkey");

            entity.HasOne(d => d.Person).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("favorites_person_id_fkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("person_pkey");

            entity.ToTable("person");

            entity.HasIndex(e => e.Email, "person_email_key").IsUnique();

            entity.HasIndex(e => e.Login, "person_login_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.City)
                .HasMaxLength(64)
                .HasColumnName("city");
            entity.Property(e => e.Email)
                .HasMaxLength(64)
                .HasColumnName("email");
            entity.Property(e => e.Login)
                .HasMaxLength(64)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(256)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(16)
                .HasColumnName("phone");
            entity.Property(e => e.PhotoName)
                .HasMaxLength(64)
                .HasColumnName("photo_name");
            entity.Property(e => e.RightId).HasColumnName("right_id");

            entity.HasOne(d => d.Right).WithMany(p => p.People)
                .HasForeignKey(d => d.RightId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("person_right_id_fkey");
        });

        modelBuilder.Entity<Right>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("right_pkey");

            entity.ToTable("right");

            entity.HasIndex(e => e.Name, "right_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
        });
    }
}