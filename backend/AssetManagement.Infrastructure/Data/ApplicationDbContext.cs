using Microsoft.EntityFrameworkCore;
using AssetManagement.Core.Entities;

namespace AssetManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Username).HasColumnName("username").IsRequired();
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
                entity.Property(e => e.Email).HasColumnName("email").IsRequired();
                entity.Property(e => e.FullName).HasColumnName("full_name").IsRequired();
                entity.Property(e => e.Role).HasColumnName("role").IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("is_active");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Asset configuration
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.ToTable("assets");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AssetTag).HasColumnName("asset_tag").IsRequired();
                entity.Property(e => e.Name).HasColumnName("name").IsRequired();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Category).HasColumnName("category").IsRequired();
                entity.Property(e => e.Status).HasColumnName("status").IsRequired();
                entity.Property(e => e.PurchaseDate).HasColumnName("purchase_date");
                entity.Property(e => e.PurchaseCost).HasColumnName("purchase_cost").HasPrecision(10, 2);
                entity.Property(e => e.AssignedToUserId).HasColumnName("assigned_to_user_id");
                entity.Property(e => e.Location).HasColumnName("location");
                entity.Property(e => e.SerialNumber).HasColumnName("serial_number");
                entity.Property(e => e.Model).HasColumnName("model");
                entity.Property(e => e.Manufacturer).HasColumnName("manufacturer");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
                entity.Property(e => e.CreatedByUserId).HasColumnName("created_by_user_id");

                entity.HasIndex(e => e.AssetTag).IsUnique();

                entity.HasOne(e => e.AssignedToUser)
                    .WithMany(u => u.AssignedAssets)
                    .HasForeignKey(e => e.AssignedToUserId);

                entity.HasOne(e => e.CreatedByUser)
                    .WithMany(u => u.CreatedAssets)
                    .HasForeignKey(e => e.CreatedByUserId);
            });

            // ActivityLog configuration
            modelBuilder.Entity<ActivityLog>(entity =>
            {
                entity.ToTable("activity_logs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AssetId).HasColumnName("asset_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Action).HasColumnName("action").IsRequired();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.HasOne(e => e.Asset)
                    .WithMany(a => a.ActivityLogs)
                    .HasForeignKey(e => e.AssetId);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.ActivityLogs)
                    .HasForeignKey(e => e.UserId);
            });

            // user and amdin
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Email = "admin@company.com",
                    FullName = "System Administrator",
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    Username = "user1",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                    Email = "user1@company.com",
                    FullName = "arveend phraseart",
                    Role = "User"
                }
            );
        }
    }
}