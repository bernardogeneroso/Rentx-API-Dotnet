using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Database;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<CarImage> CarsImages { get; set; }
    public DbSet<CarDetail> CarsDetails { get; set; }
    public DbSet<CarAppointment> CarsAppointments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var property in builder.Model.GetEntityTypes()
                 .SelectMany(t => t.GetProperties())
                 .Where
                 (p
                  => p.ClrType == typeof(DateTime)
                     || p.ClrType == typeof(DateTime?)
                 )
        )
        {
            property.SetColumnType("timestamp without time zone");
        }

        builder.Entity<RefreshToken>()
            .HasOne(u => u.User)
            .WithMany(t => t.RefreshTokens)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Car>(c => c.HasKey("Plate"));

        builder.Entity<CarImage>(c => c.HasKey(ci => new { ci.Plate, ci.ImageName }));
        builder.Entity<CarImage>()
            .HasOne(ci => ci.Car)
            .WithMany(c => c.Images)
            .HasForeignKey(ci => ci.Plate)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CarDetail>()
            .ToTable("CarsDetails")
            .HasKey(cd => cd.Plate);
        builder.Entity<CarDetail>()
            .HasOne(cd => cd.Car)
            .WithOne(c => c.Detail)
            .HasForeignKey<CarDetail>(cd => cd.Plate)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CarAppointment>()
            .ToTable("CarsAppointments")
            .HasKey(ca => ca.Id);
        builder.Entity<CarAppointment>()
            .HasOne(ca => ca.Car)
            .WithMany(c => c.Appointments)
            .HasForeignKey(ca => ca.Plate)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<CarAppointment>()
            .HasOne(ca => ca.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(ca => ca.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        AddTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow; // current datetime

            if (entity.State == EntityState.Added)
            {
                ((BaseEntity)entity.Entity).CreatedAt = now;
            }
            ((BaseEntity)entity.Entity).UpdatedAt = now;
        }
    }
}
