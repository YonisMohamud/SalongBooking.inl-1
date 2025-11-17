using SalongBooking.Domain.Entities;

namespace SalongBooking.Data.Repositories;

public interface IHairdresserRepository : IRepository<Hairdresser>
{
    Task<Hairdresser?> GetByEmailAsync(string email);
    Task<IEnumerable<Hairdresser>> GetHairdressersWithBookingsAsync();
}

