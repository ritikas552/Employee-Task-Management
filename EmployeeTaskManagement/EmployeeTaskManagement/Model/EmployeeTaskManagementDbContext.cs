using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManagement.Model;

public class EmployeeTaskManagementDbContext : IdentityDbContext<IdentityUser>
{
    public EmployeeTaskManagementDbContext(DbContextOptions<EmployeeTaskManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employee { get; set; }
    public DbSet<EmployeeTasks> Tasks { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Roles> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);

            entity.Property(e => e.FirstName)
                .HasMaxLength(50);

            entity.Property(e => e.LastName)
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasMany(e => e.EmployeeTasks)
                .WithOne(t => t.Employee)
                .HasForeignKey(t => t.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<EmployeeTasks>(entity =>
        {
            entity.HasKey(t => t.TaskId);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(t => t.Description)
                .HasMaxLength(500);

            entity.Property(t => t.Status)
                .IsRequired();

            entity.Property(t => t.AssignedFrom)
                .IsRequired();

            entity.Property(t => t.CreatedDate)
                .IsRequired();

            entity.Property(t => t.DueDate)
                .IsRequired(false);
        });

        modelBuilder.Entity<IdentityUser>(entity =>
        {
            entity.ToTable("User");
        });

        modelBuilder.Entity<IdentityRole>(entity =>
        {
            entity.ToTable("Roles");
        });
    }
}
