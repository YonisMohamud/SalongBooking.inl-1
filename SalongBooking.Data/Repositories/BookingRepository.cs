using Microsoft.EntityFrameworkCore;
using SalongBooking.Domain.Entities;
using SalongBooking.Data;

namespace SalongBooking.Data.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<Booking?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(b => b.Customer)
            .Include(b => b.Hairdresser)
            .Include(b => b.Service)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public override async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _dbSet
            .Include(b => b.Customer)
            .Include(b => b.Hairdresser)
            .Include(b => b.Service)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId)
    {
        return await _dbSet
            .Include(b => b.Hairdresser)
            .Include(b => b.Service)
            .Where(b => b.CustomerId == customerId)
            .OrderBy(b => b.BookingDate)
            .ThenBy(b => b.BookingTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByHairdresserIdAsync(int hairdresserId)
    {
        return await _dbSet
            .Include(b => b.Customer)
            .Include(b => b.Service)
            .Where(b => b.HairdresserId == hairdresserId)
            .OrderBy(b => b.BookingDate)
            .ThenBy(b => b.BookingTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date)
    {
        return await _dbSet
            .Include(b => b.Customer)
            .Include(b => b.Hairdresser)
            .Include(b => b.Service)
            .Where(b => b.BookingDate.Date == date.Date && b.Status == BookingStatus.Confirmed)
            .OrderBy(b => b.BookingTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(b => b.Customer)
            .Include(b => b.Hairdresser)
            .Include(b => b.Service)
            .Where(b => b.BookingDate >= startDate && b.BookingDate <= endDate)
            .OrderBy(b => b.BookingDate)
            .ThenBy(b => b.BookingTime)
            .ToListAsync();
    }

    public async Task<bool> IsTimeSlotAvailableAsync(int hairdresserId, DateTime bookingDate, TimeSpan bookingTime, int durationMinutes, int? excludeBookingId = null)
    {
        var bookingDateTime = bookingDate.Date.Add(bookingTime);
        var endTime = bookingDateTime.AddMinutes(durationMinutes);

        var conflictingBookings = await _dbSet
            .Where(b => b.HairdresserId == hairdresserId
                && b.BookingDate.Date == bookingDate.Date
                && b.Status == BookingStatus.Confirmed
                && (excludeBookingId == null || b.Id != excludeBookingId))
            .ToListAsync();

        foreach (var booking in conflictingBookings)
        {
            var existingStart = booking.BookingDate.Date.Add(booking.BookingTime);
            var existingEnd = existingStart.AddMinutes(booking.Service.DurationMinutes);

            // Check for overlap
            if (bookingDateTime < existingEnd && endTime > existingStart)
            {
                return false;
            }
        }

        return true;
    }
}

