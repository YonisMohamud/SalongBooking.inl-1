using SalongBooking.DTOs;

namespace SalongBooking.Services.Interfaces;

public interface IBookingService
{
    Task<BookingDto?> GetByIdAsync(int id);
    Task<PagedResultDto<BookingDto>> GetAllAsync(int page = 1, int pageSize = 10, string? filter = null, string? sort = "asc");
    Task<BookingDto> CreateAsync(CreateBookingDto dto);
    Task<BookingDto?> UpdateAsync(int id, UpdateBookingDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> CancelBookingAsync(int id);
    Task<IEnumerable<BookingDto>> GetBookingsByCustomerIdAsync(int customerId);
    Task<IEnumerable<BookingDto>> GetBookingsByHairdresserIdAsync(int hairdresserId);
    Task<IEnumerable<BookingDto>> GetBookingsByDateAsync(DateTime date);
}

