using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class PaymentMethodRepository(OrderContext context) : BaseRepository<PaymentMethodEntity>(context), IPaymentMethodRepository
{
    private readonly OrderContext _context = context;
}
