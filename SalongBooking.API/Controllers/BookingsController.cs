using Microsoft.AspNetCore.Mvc;
using SalongBooking.DTOs;
using SalongBooking.Services.Interfaces;

namespace SalongBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _service;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(IBookingService service, ILogger<BookingsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<BookingDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filter = null,
        [FromQuery] string? sort = "asc")
    {
        var result = await _service.GetAllAsync(page, pageSize, filter, sort);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetById(int id)
    {
        var booking = await _service.GetByIdAsync(id);
        if (booking == null)
            return NotFound();

        return Ok(booking);
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingDto dto)
    {
        try
        {
            var booking = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BookingDto>> Update(int id, [FromBody] UpdateBookingDto dto)
    {
        try
        {
            var booking = await _service.UpdateAsync(id, dto);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var cancelled = await _service.CancelBookingAsync(id);
        if (!cancelled)
            return NotFound();

        return Ok(new { message = "Bokningen är avbokad" });
    }

    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetByCustomer(int customerId)
    {
        var bookings = await _service.GetBookingsByCustomerIdAsync(customerId);
        return Ok(bookings);
    }

    [HttpGet("hairdresser/{hairdresserId}")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetByHairdresser(int hairdresserId)
    {
        var bookings = await _service.GetBookingsByHairdresserIdAsync(hairdresserId);
        return Ok(bookings);
    }

    [HttpGet("date/{date:datetime}")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetByDate(DateTime date)
    {
        var bookings = await _service.GetBookingsByDateAsync(date);
        return Ok(bookings);
    }
}

