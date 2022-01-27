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
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

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

        builder.Entity<Car>(c => c.HasKey("Plate"));


        builder.Entity<CarImage>(c => c.HasKey(ci => new { ci.Plate, ci.ImageName }));
        builder.Entity<CarImage>()
            .HasOne(ci => ci.Car)
            .WithMany(c => c.CarImages)
            .HasForeignKey(ci => ci.Plate)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CarDetail>().ToTable("CarsDetails").HasKey(cd => cd.Plate);
        builder.Entity<CarDetail>()
            .HasOne(cd => cd.Car)
            .WithOne(c => c.CarDetails)
            .HasForeignKey<CarDetail>(cd => cd.Plate)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<CarAppointment>().ToTable("CarsAppointments").HasKey(ca => ca.Id);
        builder.Entity<CarAppointment>()
            .HasOne(ca => ca.Car)
            .WithMany(c => c.CarAppointments)
            .HasForeignKey(ca => ca.Plate)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<CarAppointment>()
            .HasOne(ca => ca.User)
            .WithMany(u => u.CarAppointments)
            .HasForeignKey(ca => ca.UserId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
