namespace SalongBooking.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan BookingTime { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign keys
    public int CustomerId { get; set; }
    public int HairdresserId { get; set; }
    public int ServiceId { get; set; }
    
    // Navigation properties
    public Customer Customer { get; set; } = null!;
    public Hairdresser Hairdresser { get; set; } = null!;
    public Service Service { get; set; } = null!;
}

public enum BookingStatus
{
    Confirmed = 1,
    Cancelled = 2,
    Completed = 3,
    NoShow = 4
}

