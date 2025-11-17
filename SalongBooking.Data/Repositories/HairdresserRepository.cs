using Microsoft.EntityFrameworkCore;
using SalongBooking.Domain.Entities;
using SalongBooking.Data;

namespace SalongBooking.Data.Repositories;

public class HairdresserRepository : Repository<Hairdresser>, IHairdresserRepository
{
    public HairdresserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Hairdresser?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(h => h.Email == email);
    }

    public async Task<IEnumerable<Hairdresser>> GetHairdressersWithBookingsAsync()
    {
        return await _dbSet
            .Include(h => h.Bookings)
            .ThenInclude(b => b.Service)
            .Include(h => h.Bookings)
            .ThenInclude(b => b.Customer)
            .ToListAsync();
    }
}

