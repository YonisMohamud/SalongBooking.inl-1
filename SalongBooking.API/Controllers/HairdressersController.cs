using Microsoft.AspNetCore.Mvc;
using SalongBooking.DTOs;
using SalongBooking.Services.Interfaces;

namespace SalongBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HairdressersController : ControllerBase
{
    private readonly IHairdresserService _service;
    private readonly ILogger<HairdressersController> _logger;

    public HairdressersController(IHairdresserService service, ILogger<HairdressersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<HairdresserDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? filter = null,
        [FromQuery] string? sort = "asc")
    {
        var result = await _service.GetAllAsync(page, pageSize, filter, sort);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HairdresserDto>> GetById(int id)
    {
        var hairdresser = await _service.GetByIdAsync(id);
        if (hairdresser == null)
            return NotFound();

        return Ok(hairdresser);
    }

    [HttpPost]
    public async Task<ActionResult<HairdresserDto>> Create([FromBody] CreateHairdresserDto dto)
    {
        var hairdresser = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = hairdresser.Id }, hairdresser);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<HairdresserDto>> Update(int id, [FromBody] UpdateHairdresserDto dto)
    {
        var hairdresser = await _service.UpdateAsync(id, dto);
        if (hairdresser == null)
            return NotFound();

        return Ok(hairdresser);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

