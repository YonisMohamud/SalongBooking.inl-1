namespace SalongBooking.Domain.Entities;

public class Customer : User
{
    public string PhoneNumber { get; set; } = string.Empty;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

