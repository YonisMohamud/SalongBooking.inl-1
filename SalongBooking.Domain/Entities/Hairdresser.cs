namespace SalongBooking.Domain.Entities;

public class Hairdresser : User
{
    public string Specialization { get; set; } = string.Empty;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

