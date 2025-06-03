using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class CustomerRepository(OrderContext context) : BaseRepository<CustomerEntity>(context), ICustomerRepository
{
    private readonly OrderContext _context = context;
}
