using Bogus;
using Microsoft.EntityFrameworkCore;
using SalongBooking.Data;
using SalongBooking.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace SalongBooking.API;

public static class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Only seed if database is empty
        if (await context.Customers.AnyAsync() || 
            await context.Hairdressers.AnyAsync() || 
            await context.Services.AnyAsync())
        {
            return;
        }

        var faker = new Faker("sv");

        // Seed Services
        var services = new List<Service>
        {
            new Service { Name = "Herrklippning", Price = 350, DurationMinutes = 30, Description = "Klassisk herrklippning", CreatedAt = DateTime.UtcNow },
            new Service { Name = "Damklippning", Price = 550, DurationMinutes = 60, Description = "Professionell damklippning", CreatedAt = DateTime.UtcNow },
            new Service { Name = "Färgning", Price = 1200, DurationMinutes = 120, Description = "Fullständig färgning", CreatedAt = DateTime.UtcNow },
            new Service { Name = "Högljning", Price = 800, DurationMinutes = 90, Description = "Högljning med toner", CreatedAt = DateTime.UtcNow },
            new Service { Name = "Styling", Price = 450, DurationMinutes = 45, Description = "Styling och formgivning", CreatedAt = DateTime.UtcNow }
        };
        await context.Services.AddRangeAsync(services);
        await context.SaveChangesAsync();

        // Seed Hairdressers
        var hairdresserFaker = new Faker<Hairdresser>("sv")
            .RuleFor(h => h.Name, f => f.Name.FullName())
            .RuleFor(h => h.Email, f => f.Internet.Email())
            .RuleFor(h => h.PasswordHash, f => HashPassword("Password123!"))
            .RuleFor(h => h.Specialization, f => f.PickRandom("Herrklippning", "Damklippning", "Färgning", "Styling", "Allmänt"))
            .RuleFor(h => h.CreatedAt, f => DateTime.UtcNow);

        var hairdressers = hairdresserFaker.Generate(5);
        await context.Hairdressers.AddRangeAsync(hairdressers);
        await context.SaveChangesAsync();

        // Seed Customers
        var customerFaker = new Faker<Customer>("sv")
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.PasswordHash, f => HashPassword("Password123!"))
            .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber("07########"))
            .RuleFor(c => c.CreatedAt, f => DateTime.UtcNow);

        var customers = customerFaker.Generate(20);
        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();

        // Seed Bookings
        var bookingFaker = new Faker<Booking>("sv")
            .RuleFor(b => b.BookingDate, f => f.Date.Future(30))
            .RuleFor(b => b.BookingTime, f => new TimeSpan(9 + f.Random.Int(0, 8), f.Random.Int(0, 1) * 30, 0))
            .RuleFor(b => b.Status, f => f.PickRandom<BookingStatus>())
            .RuleFor(b => b.Notes, f => f.Random.Bool(0.3f) ? f.Lorem.Sentence() : null)
            .RuleFor(b => b.CustomerId, f => f.PickRandom(customers).Id)
            .RuleFor(b => b.HairdresserId, f => f.PickRandom(hairdressers).Id)
            .RuleFor(b => b.ServiceId, f => f.PickRandom(services).Id)
            .RuleFor(b => b.CreatedAt, f => DateTime.UtcNow);

        var bookings = bookingFaker.Generate(50);
        await context.Bookings.AddRangeAsync(bookings);
        await context.SaveChangesAsync();
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

