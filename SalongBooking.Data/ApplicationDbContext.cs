using Microsoft.EntityFrameworkCore;
using SalongBooking.Domain.Entities;

namespace SalongBooking.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Hairdresser> Hairdressers { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User inheritance (TPH - Table Per Hierarchy)
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.HasDiscriminator<string>("UserType")
                .HasValue<Customer>("Customer")
                .HasValue<Hairdresser>("Hairdresser");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.HasIndex(e => e.Email)
                .IsUnique();
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
        });

        // Configure Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20);
        });

        // Configure Hairdresser
        modelBuilder.Entity<Hairdresser>(entity =>
        {
            entity.Property(e => e.Specialization)
                .HasMaxLength(100);
        });

        // Configure Service
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Price)
                .HasPrecision(18, 2);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
        });

        // Configure Booking
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.BookingDate)
                .IsRequired();
            
            entity.Property(e => e.BookingTime)
                .IsRequired();
            
            entity.Property(e => e.Status)
                .HasConversion<int>();

            // Relationships
            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Hairdresser)
                .WithMany(h => h.Bookings)
                .HasForeignKey(e => e.HairdresserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(e => e.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => new { e.BookingDate, e.BookingTime, e.HairdresserId });
            entity.HasIndex(e => e.CustomerId);
        });
    }
}

