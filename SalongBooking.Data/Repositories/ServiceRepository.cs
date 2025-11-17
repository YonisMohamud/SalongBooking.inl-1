using Microsoft.EntityFrameworkCore;
using SalongBooking.Domain.Entities;
using SalongBooking.Data;

namespace SalongBooking.Data.Repositories;

public class ServiceRepository : Repository<Service>, IServiceRepository
{
    public ServiceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Service>> GetServicesWithBookingsAsync()
    {
        return await _dbSet
            .Include(s => s.Bookings)
            .ThenInclude(b => b.Customer)
            .Include(s => s.Bookings)
            .ThenInclude(b => b.Hairdresser)
            .ToListAsync();
    }
}

