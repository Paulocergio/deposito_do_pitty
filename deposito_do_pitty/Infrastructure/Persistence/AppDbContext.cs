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

        public DbSet<Budget> Budgets { get; set; } = null!;
        public DbSet<BudgetItem> BudgetItems { get; set; } = null!;
        public DbSet<Product> Products { get; set; }
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

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.ToTable("budgets");
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Id)
                    .HasColumnName("id");

                entity.Property(b => b.BudgetNumber)
                    .HasColumnName("budgetnumber")
                    .HasMaxLength(50);

                entity.Property(b => b.CustomerName)
                    .HasColumnName("customername")
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(b => b.Email)
                    .HasColumnName("email")
                    .HasMaxLength(150);

                entity.Property(b => b.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(20);

                entity.Property(b => b.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255);

                entity.Property(b => b.IssueDate)
                    .HasColumnName("issuedate")
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );


                entity.Property(b => b.DueDate)
                    .HasColumnName("duedate")
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v
                    );

                entity.Property(b => b.Discount)
                    .HasColumnName("discount")
                    .HasPrecision(10, 2);

                entity.Property(b => b.Total)
                    .HasColumnName("total")
                    .HasPrecision(10, 2);

                entity.Property(b => b.CreatedAt)
                    .HasColumnName("createdat")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );
                entity.Property(b => b.UpdatedAt)
                    .HasColumnName("updatedat")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasConversion(
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null,
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null
                    );


                entity.HasMany(b => b.Items)
                    .WithOne(i => i.Budget!)
                    .HasForeignKey(i => i.BudgetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


           


            modelBuilder.Entity<BudgetItem>(entity =>
            {
                entity.ToTable("budgetitems");
                entity.HasKey(i => i.Id);

                entity.Property(i => i.Id).HasColumnName("id");
                entity.Property(i => i.BudgetId).HasColumnName("budgetid");
                entity.Property(i => i.Description).HasColumnName("description");
                entity.Property(i => i.Quantity).HasColumnName("quantity");
                entity.Property(i => i.UnitPrice).HasColumnName("unitprice").HasPrecision(10, 2);
                entity.Property(i => i.Total).HasColumnName("total").HasPrecision(10, 2);
            });



            modelBuilder.Entity<Client>(entity =>
            {
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

            modelBuilder.Entity<Supplier>(entity =>
            {
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


            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .HasColumnName("id");

                entity.Property(p => p.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(p => p.Description)
                    .HasColumnName("description");

                entity.Property(p => p.PurchasePrice)
                    .HasColumnName("purchaseprice")
                    .HasPrecision(10, 2);

                entity.Property(p => p.SalePrice)
                    .HasColumnName("saleprice")
                    .HasPrecision(10, 2);

                entity.Property(p => p.Category)
                    .HasColumnName("category")
                    .HasMaxLength(100);

                entity.Property(p => p.StockQuantity)
                    .HasColumnName("stockquantity");

                entity.Property(p => p.Status)
                    .HasColumnName("status")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(p => p.Barcode)
                    .HasColumnName("barcode")
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(p => p.CreatedAt)
                    .HasColumnName("createdat")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.Property(p => p.UpdatedAt)
                    .HasColumnName("updatedat")
                    .HasColumnType("timestamp with time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    );

                entity.HasIndex(p => p.Barcode).IsUnique();
            });

        }
    }
}
