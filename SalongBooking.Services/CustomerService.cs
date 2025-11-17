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

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public CustomerService(ICustomerRepository repository, IMapper mapper, ApplicationDbContext context)
    {
        _repository = repository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<PagedResultDto<CustomerDto>> GetAllAsync(int page = 1, int pageSize = 10, string? filter = null, string? sort = "asc")
    {
        var query = _context.Set<Customer>().AsQueryable();

        // Apply filter
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(c => 
                c.Name.Contains(filter) || 
                c.Email.Contains(filter) || 
                c.PhoneNumber.Contains(filter));
        }

        // Apply sorting
        query = sort?.ToLower() == "desc" 
            ? query.OrderByDescending(c => c.Name)
            : query.OrderBy(c => c.Name);

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var customers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<CustomerDto>
        {
            Items = _mapper.Map<IEnumerable<CustomerDto>>(customers),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = _mapper.Map<Customer>(dto);
        customer.PasswordHash = HashPassword(dto.Password);
        customer.CreatedAt = DateTime.UtcNow;

        var created = await _repository.AddAsync(customer);
        return _mapper.Map<CustomerDto>(created);
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null)
            return null;

        _mapper.Map(dto, customer);
        customer.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(customer);
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null)
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

