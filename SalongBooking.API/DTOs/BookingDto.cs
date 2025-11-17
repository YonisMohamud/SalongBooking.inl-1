namespace SalongBooking.API.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan BookingTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public CustomerDto Customer { get; set; } = null!;
    public HairdresserDto Hairdresser { get; set; } = null!;
    public ServiceDto Service { get; set; } = null!;
}

public class CreateBookingDto
{
    public DateTime BookingDate { get; set; }
    public TimeSpan BookingTime { get; set; }
    public int CustomerId { get; set; }
    public int HairdresserId { get; set; }
    public int ServiceId { get; set; }
    public string? Notes { get; set; }
}

public class UpdateBookingDto
{
    public DateTime? BookingDate { get; set; }
    public TimeSpan? BookingTime { get; set; }
    public int? HairdresserId { get; set; }
    public int? ServiceId { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}

