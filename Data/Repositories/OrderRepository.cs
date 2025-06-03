using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class OrderRepsitory(OrderContext context) : BaseRepository<OrderEntity>(context), IOrderRepository
{
    private readonly OrderContext _context = context;
}
