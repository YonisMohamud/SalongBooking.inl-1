using SalongBooking.Domain.Entities;

namespace SalongBooking.Data.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<IEnumerable<Customer>> GetCustomersWithBookingsAsync();
}

