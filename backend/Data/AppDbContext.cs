using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Ticket> Activities => Set<Ticket>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(160).IsRequired();
            entity.Property(x => x.Login).HasColumnName("login").HasMaxLength(80).IsRequired();
            entity.Property(x => x.Role).HasColumnName("role").IsRequired();
            entity.HasIndex(x => x.Login).IsUnique();
            entity.HasCheckConstraint("users_role_check", "role IN ('Student', 'Reviewer', 'Manager')");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
            entity.HasIndex(x => x.Name).IsUnique();
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("activities");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Id).HasColumnName("id").UseIdentityByDefaultColumn();
            entity.Property(x => x.Number).HasColumnName("number").UseIdentityByDefaultColumn();
            entity.Property(x => x.Title).HasColumnName("title").HasMaxLength(80).IsRequired();
            entity.Property(x => x.Description).HasColumnName("description").HasMaxLength(500).IsRequired();
            entity.Property(x => x.CategoryId).HasColumnName("category_id");
            entity.Property(x => x.Status).HasColumnName("status").IsRequired();
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            entity.Property(x => x.CreatedByUserId).HasColumnName("created_by_user_id");
            entity.Property(x => x.AssigneeUserId).HasColumnName("assignee_user_id");
            entity.HasIndex(x => x.Status).HasDatabaseName("activities_status_idx");
            entity.HasIndex(x => x.CategoryId).HasDatabaseName("activities_category_id_idx");
            entity.HasCheckConstraint("activities_title_check", "char_length(title) BETWEEN 5 AND 80");
            entity.HasCheckConstraint("activities_description_check", "char_length(description) BETWEEN 10 AND 500");
            entity.HasCheckConstraint("activities_status_check", "status IN ('New', 'InProgress', 'Closed', 'Cancelled')");
            entity.HasOne(x => x.Category).WithMany(x => x.Activities)
                .HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.CreatedByUser).WithMany(x => x.CreatedActivities)
                .HasForeignKey(x => x.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.AssigneeUser).WithMany(x => x.AssignedActivities)
                .HasForeignKey(x => x.AssigneeUserId).OnDelete(DeleteBehavior.SetNull);
        });
    }
}
