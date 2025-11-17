using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SalongBooking.DTOs;
using SalongBooking.Data;
using SalongBooking.Data.Repositories;
using SalongBooking.Domain.Entities;
using SalongBooking.Services.Interfaces;

namespace SalongBooking.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IHairdresserRepository _hairdresserRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public BookingService(
        IBookingRepository repository,
        ICustomerRepository customerRepository,
        IHairdresserRepository hairdresserRepository,
        IServiceRepository serviceRepository,
        IEmailService emailService,
        IMapper mapper,
        ApplicationDbContext context)
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _hairdresserRepository = hairdresserRepository;
        _serviceRepository = serviceRepository;
        _emailService = emailService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<BookingDto?> GetByIdAsync(int id)
    {
        var booking = await _repository.GetByIdAsync(id);
        return booking == null ? null : _mapper.Map<BookingDto>(booking);
    }

    public async Task<PagedResultDto<BookingDto>> GetAllAsync(int page = 1, int pageSize = 10, string? filter = null, string? sort = "asc")
    {
        var query = _context.Set<Booking>()
            .Include(b => b.Customer)
            .Include(b => b.Hairdresser)
            .Include(b => b.Service)
            .AsQueryable();

        // Apply filter
        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(b => 
                b.Customer.Name.Contains(filter) || 
                b.Hairdresser.Name.Contains(filter) || 
                b.Service.Name.Contains(filter) ||
                b.Status.ToString().Contains(filter));
        }

        // Apply sorting
        query = sort?.ToLower() == "desc" 
            ? query.OrderByDescending(b => b.BookingDate).ThenByDescending(b => b.BookingTime)
            : query.OrderBy(b => b.BookingDate).ThenBy(b => b.BookingTime);

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination
        var bookings = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<BookingDto>
        {
            Items = _mapper.Map<IEnumerable<BookingDto>>(bookings),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<BookingDto> CreateAsync(CreateBookingDto dto)
    {
        // Validate entities exist
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            throw new ArgumentException("Kund hittades inte");

        var hairdresser = await _hairdresserRepository.GetByIdAsync(dto.HairdresserId);
        if (hairdresser == null)
            throw new ArgumentException("Frisör hittades inte");

        var service = await _serviceRepository.GetByIdAsync(dto.ServiceId);
        if (service == null)
            throw new ArgumentException("Tjänst hittades inte");

        // Check availability
        var isAvailable = await _repository.IsTimeSlotAvailableAsync(
            dto.HairdresserId, 
            dto.BookingDate, 
            dto.BookingTime, 
            service.DurationMinutes);

        if (!isAvailable)
            throw new InvalidOperationException("Tidsperioden är inte tillgänglig");

        var booking = _mapper.Map<Booking>(dto);
        booking.CreatedAt = DateTime.UtcNow;

        var created = await _repository.AddAsync(booking);

        // Send confirmation email
        _ = Task.Run(async () =>
        {
            await _emailService.SendBookingConfirmationAsync(
                customer.Email,
                customer.Name,
                booking.BookingDate,
                booking.BookingTime,
                service.Name);
        });

        return _mapper.Map<BookingDto>(created);
    }

    public async Task<BookingDto?> UpdateAsync(int id, UpdateBookingDto dto)
    {
        var booking = await _repository.GetByIdAsync(id);
        if (booking == null)
            return null;

        // If date/time or hairdresser/service changed, check availability
        if (dto.BookingDate.HasValue || dto.BookingTime.HasValue || dto.HairdresserId.HasValue || dto.ServiceId.HasValue)
        {
            var bookingDate = dto.BookingDate ?? booking.BookingDate;
            var bookingTime = dto.BookingTime ?? booking.BookingTime;
            var hairdresserId = dto.HairdresserId ?? booking.HairdresserId;
            var serviceId = dto.ServiceId ?? booking.ServiceId;

            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null)
                throw new ArgumentException("Tjänst hittades inte");

            var isAvailable = await _repository.IsTimeSlotAvailableAsync(
                hairdresserId,
                bookingDate,
                bookingTime,
                service.DurationMinutes,
                id);

            if (!isAvailable)
                throw new InvalidOperationException("Tidsperioden är inte tillgänglig");
        }

        _mapper.Map(dto, booking);
        booking.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(booking);
        return _mapper.Map<BookingDto>(booking);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var booking = await _repository.GetByIdAsync(id);
        if (booking == null)
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> CancelBookingAsync(int id)
    {
        var booking = await _repository.GetByIdAsync(id);
        if (booking == null)
            return false;

        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(booking);
        return true;
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByCustomerIdAsync(int customerId)
    {
        var bookings = await _repository.GetBookingsByCustomerIdAsync(customerId);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByHairdresserIdAsync(int hairdresserId)
    {
        var bookings = await _repository.GetBookingsByHairdresserIdAsync(hairdresserId);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }

    public async Task<IEnumerable<BookingDto>> GetBookingsByDateAsync(DateTime date)
    {
        var bookings = await _repository.GetBookingsByDateAsync(date);
        return _mapper.Map<IEnumerable<BookingDto>>(bookings);
    }
}

