using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalongBooking.DTOs;
using SalongBooking.Data;
using SalongBooking.Data.Repositories;
using SalongBooking.Domain.Entities;
using SalongBooking.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SalongBooking.Services;

public class HairdresserService : IHairdresserService
{
    private readonly IHairdresserRepository _repository;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public HairdresserService(IHairdresserRepository repository, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<HairdresserDto?> GetByIdAsync(int id)
    {
        var hairdresser = await _repository.GetByIdAsync(id);
        return hairdresser == null ? null : _mapper.Map<HairdresserDto>(hairdresser);
    }

    public async Task<PagedResultDto<HairdresserDto>> GetAllAsync(int page = 1, int pageSize = 10, string? filter = null, string? sort = "asc")
    {
        var query = _context.Set<Hairdresser>().AsQueryable();

        // Apply filter
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(h => 
                h.Name.Contains(filter) || 
                h.Email.Contains(filter) || 
                h.Specialization.Contains(filter));
        }

        // Apply sorting
        query = sort?.ToLower() == "desc" 
            ? query.OrderByDescending(h => h.Name)
            : query.OrderBy(h => h.Name);

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var hairdressers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<HairdresserDto>
        {
            Items = _mapper.Map<IEnumerable<HairdresserDto>>(hairdressers),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<HairdresserDto> CreateAsync(CreateHairdresserDto dto)
    {
        var hairdresser = _mapper.Map<Hairdresser>(dto);
        hairdresser.PasswordHash = HashPassword(dto.Password);
        hairdresser.CreatedAt = DateTime.UtcNow;

        var created = await _repository.AddAsync(hairdresser);
        return _mapper.Map<HairdresserDto>(created);
    }

    public async Task<HairdresserDto?> UpdateAsync(int id, UpdateHairdresserDto dto)
    {
        var hairdresser = await _repository.GetByIdAsync(id);
        if (hairdresser == null)
            return null;

        _mapper.Map(dto, hairdresser);
        hairdresser.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(hairdresser);
        return _mapper.Map<HairdresserDto>(hairdresser);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var hairdresser = await _repository.GetByIdAsync(id);
        if (hairdresser == null)
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }
}

