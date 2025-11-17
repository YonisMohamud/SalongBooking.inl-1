using System.Text.Json;
using Microsoft.Extensions.Logging;
using SalongBooking.Services.Interfaces;

namespace SalongBooking.Services;

public class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<EmailService> _logger;
    private const string EmailApiUrl = "https://jsonplaceholder.typicode.com/posts"; // Mock API för demonstration

    public EmailService(HttpClient httpClient, ILogger<EmailService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> SendBookingConfirmationAsync(string toEmail, string customerName, DateTime bookingDate, TimeSpan bookingTime, string serviceName)
    {
        try
        {
            var emailData = new
            {
                to = toEmail,
                subject = "Bokningsbekräftelse - SalongBooking",
                body = $"Hej {customerName},\n\nDin bokning för {serviceName} är bekräftad.\n\nDatum: {bookingDate:yyyy-MM-dd}\nTid: {bookingTime:hh\\:mm}\n\nVälkommen!",
                type = "confirmation"
            };

            var json = JsonSerializer.Serialize(emailData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(EmailApiUrl, content);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Bokningsbekräftelse skickad till {Email}", toEmail);
                return true;
            }

            _logger.LogWarning("Kunde inte skicka bekräftelse till {Email}. Status: {Status}", toEmail, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fel vid skickande av bokningsbekräftelse till {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendBookingReminderAsync(string toEmail, string customerName, DateTime bookingDate, TimeSpan bookingTime, string serviceName)
    {
        try
        {
            var emailData = new
            {
                to = toEmail,
                subject = "Påminnelse - SalongBooking",
                body = $"Hej {customerName},\n\nDetta är en påminnelse om din bokning för {serviceName}.\n\nDatum: {bookingDate:yyyy-MM-dd}\nTid: {bookingTime:hh\\:mm}\n\nVi ser fram emot ditt besök!",
                type = "reminder"
            };

            var json = JsonSerializer.Serialize(emailData);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(EmailApiUrl, content);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Påminnelse skickad till {Email}", toEmail);
                return true;
            }

            _logger.LogWarning("Kunde inte skicka påminnelse till {Email}. Status: {Status}", toEmail, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fel vid skickande av påminnelse till {Email}", toEmail);
            return false;
        }
    }
}

