using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalongBooking.DTOs;
using SalongBooking.Data;
using SalongBooking.Data.Repositories;
using SalongBooking.Domain.Entities;
using SalongBooking.Services.Interfaces;

namespace SalongBooking.Services;

public class ServiceService : IServiceService
{
    private readonly IServiceRepository _repository;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public ServiceService(IServiceRepository repository, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<ServiceDto?> GetByIdAsync(int id)
    {
        var service = await _repository.GetByIdAsync(id);
        return service == null ? null : _mapper.Map<ServiceDto>(service);
    }

    public async Task<PagedResultDto<ServiceDto>> GetAllAsync(int page = 1, int pageSize = 10, string? filter = null, string? sort = "asc")
    {
        var query = _context.Set<Service>().AsQueryable();

        // Apply filter
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(s => 
                s.Name.Contains(filter) || 
                s.Description.Contains(filter));
        }

        // Apply sorting
        query = sort?.ToLower() == "desc" 
            ? query.OrderByDescending(s => s.Name)
            : query.OrderBy(s => s.Name);

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var services = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<ServiceDto>
        {
            Items = _mapper.Map<IEnumerable<ServiceDto>>(services),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
    {
        var service = _mapper.Map<Service>(dto);
        service.CreatedAt = DateTime.UtcNow;

        var created = await _repository.AddAsync(service);
        return _mapper.Map<ServiceDto>(created);
    }

    public async Task<ServiceDto?> UpdateAsync(int id, UpdateServiceDto dto)
    {
        var service = await _repository.GetByIdAsync(id);
        if (service == null)
            return null;

        _mapper.Map(dto, service);
        service.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(service);
        return _mapper.Map<ServiceDto>(service);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var service = await _repository.GetByIdAsync(id);
        if (service == null)
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }
}

