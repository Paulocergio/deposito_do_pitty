using deposito_do_pitty.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DepositoDoPitty.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Name).HasColumnName("name");
                entity.Property(u => u.Email).HasColumnName("email");
                entity.Property(u => u.PasswordHash).HasColumnName("password_hash");
                entity.Property(u => u.Phone).HasColumnName("phone");
                entity.Property(u => u.Role).HasColumnName("role").IsRequired();



                entity.Property(u => u.IsActive).HasColumnName("is_active");
            });

        }
    }
}
