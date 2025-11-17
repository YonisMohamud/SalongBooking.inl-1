using SalongBooking.Domain.Entities;

namespace SalongBooking.Data.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByCustomerIdAsync(int customerId);
    Task<IEnumerable<Booking>> GetBookingsByHairdresserIdAsync(int hairdresserId);
    Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date);
    Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<bool> IsTimeSlotAvailableAsync(int hairdresserId, DateTime bookingDate, TimeSpan bookingTime, int durationMinutes, int? excludeBookingId = null);
}

