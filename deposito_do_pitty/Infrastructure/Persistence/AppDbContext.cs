using deposito_do_pitty.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DepositoDoPitty.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Client> Clients { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
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

            modelBuilder.Entity<Client>(entity => {
                entity.ToTable("clients");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();

                entity.Property(e => e.DocumentNumber)
                  .HasColumnName("document_number")
                  .HasMaxLength(20)
                  .IsRequired();

                entity.Property(e => e.CompanyName)
                  .HasColumnName("company_name")
                  .HasMaxLength(150)
                  .IsRequired();

                entity.Property(e => e.Phone)
                  .HasColumnName("phone")
                  .HasMaxLength(20);

                entity.Property(e => e.Email)
                  .HasColumnName("email")
                  .HasMaxLength(150);

                entity.Property(e => e.Address)
                  .HasColumnName("address")
                  .HasMaxLength(255);

                entity.Property(e => e.PostalCode)
                  .HasColumnName("postal_code")
                  .HasMaxLength(10);

                entity.Property(e => e.ContactPerson)
                  .HasColumnName("contact_person")
                  .HasMaxLength(100);

                entity.Property(e => e.IsActive)
                  .HasColumnName("is_active")
                  .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                  .HasColumnName("created_at")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                  .HasColumnName("updated_at")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            ;

            modelBuilder.Entity<Supplier>(entity => {
                entity.ToTable("suppliers");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                  .HasColumnName("id")
                  .ValueGeneratedOnAdd();

                entity.Property(e => e.DocumentNumber)
                  .HasColumnName("document_number")
                  .HasMaxLength(20)
                  .IsRequired();

                entity.Property(e => e.CompanyName)
                  .HasColumnName("company_name")
                  .HasMaxLength(255);

                entity.Property(e => e.Address)
                  .HasColumnName("address")
                  .HasMaxLength(255);

                entity.Property(e => e.Number)
                  .HasColumnName("number")
                  .HasMaxLength(20);

                entity.Property(e => e.Neighborhood)
                  .HasColumnName("neighborhood")
                  .HasMaxLength(100);

                entity.Property(e => e.City)
                  .HasColumnName("city")
                  .HasMaxLength(100);

                entity.Property(e => e.State)
                  .HasColumnName("state")
                  .HasMaxLength(2);

                entity.Property(e => e.PostalCode)
                  .HasColumnName("postal_code")
                  .HasMaxLength(10);

                entity.Property(e => e.Phone)
                  .HasColumnName("phone")
                  .HasMaxLength(20);

                entity.Property(e => e.RegistrationStatus)
                  .HasColumnName("registration_status")
                  .HasMaxLength(50);

                entity.Property(e => e.BranchType)
                  .HasColumnName("branch_type")
                  .HasMaxLength(50);

                entity.Property(e => e.Email)
                  .HasColumnName("email")
                  .HasMaxLength(150);

                entity.Property(e => e.CreatedAt)
                  .HasColumnName("created_at")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                  );

                entity.Property(e => e.UpdatedAt)
                  .HasColumnName("updated_at")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP")
                  .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                  );

            });
        }
    }
}
