using Microsoft.EntityFrameworkCore;
using SalongBooking.Domain.Entities;
using SalongBooking.Data;

namespace SalongBooking.Data.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithBookingsAsync()
    {
        return await _dbSet
            .Include(c => c.Bookings)
            .ThenInclude(b => b.Service)
            .Include(c => c.Bookings)
            .ThenInclude(b => b.Hairdresser)
            .ToListAsync();
    }
}

