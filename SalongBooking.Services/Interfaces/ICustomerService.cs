using SalongBooking.DTOs;

namespace SalongBooking.Services.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<PagedResultDto<CustomerDto>> GetAllAsync(int page = 1, int pageSize = 10, string? filter = null, string? sort = "asc");
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}

