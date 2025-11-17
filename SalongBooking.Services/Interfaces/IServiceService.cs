using SalongBooking.DTOs;

namespace SalongBooking.Services.Interfaces;

public interface IServiceService
{
    Task<ServiceDto?> GetByIdAsync(int id);
    Task<PagedResultDto<ServiceDto>> GetAllAsync(int page = 1, int pageSize = 10, string? filter = null, string? sort = "asc");
    Task<ServiceDto> CreateAsync(CreateServiceDto dto);
    Task<ServiceDto?> UpdateAsync(int id, UpdateServiceDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

