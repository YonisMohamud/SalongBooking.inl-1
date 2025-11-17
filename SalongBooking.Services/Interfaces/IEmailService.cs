namespace SalongBooking.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendBookingConfirmationAsync(string toEmail, string customerName, DateTime bookingDate, TimeSpan bookingTime, string serviceName);
    Task<bool> SendBookingReminderAsync(string toEmail, string customerName, DateTime bookingDate, TimeSpan bookingTime, string serviceName);
}

