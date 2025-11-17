using SalongBooking.Domain.Entities;

namespace SalongBooking.Data.Repositories;

public interface IServiceRepository : IRepository<Service>
{
    Task<IEnumerable<Service>> GetServicesWithBookingsAsync();
}

