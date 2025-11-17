using SalongBooking.DTOs;

namespace SalongBooking.Services.Interfaces;

public interface IHairdresserService
{
    Task<HairdresserDto?> GetByIdAsync(int id);
    Task<PagedResultDto<HairdresserDto>> GetAllAsync(int page = 1, int pageSize = 10, string? filter = null, string? sort = "asc");
    Task<HairdresserDto> CreateAsync(CreateHairdresserDto dto);
    Task<HairdresserDto?> UpdateAsync(int id, UpdateHairdresserDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

