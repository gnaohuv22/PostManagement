using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.Extensions.Configuration;

namespace WebApplication1.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUser { get; set; }
    public virtual DbSet<PostCategories> PostCategories { get; set; }
    public virtual DbSet<Posts> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var conf = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(conf.GetConnectionString("DbConnection"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostCategories>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.Property(e => e.CategoryName).HasMaxLength(50);

            entity.Property(e => e.Description).HasMaxLength(200);
        });

        modelBuilder.Entity<Posts>(entity =>
        {
            entity.HasKey(e => e.PostId);

            entity.HasIndex(e => e.CategoryId, "IX_Posts_CategoryId");

            entity.HasIndex(e => e.AuthorId, "IX_Posts_AuthorId");

            entity.HasOne(d => d.AppUser).WithMany(p => p.Posts).HasForeignKey(d => d.AuthorId);

            entity.HasOne(d => d.PostCategories).WithMany(p => p.Posts).HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.Address).HasMaxLength(200);

            entity.Property(e => e.Email).HasMaxLength(50);

            entity.Property(e => e.Password).HasMaxLength(50);

            entity.Property(e => e.Fullname).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
