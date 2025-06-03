using Data.Entities;
using Presentation.Models;

namespace Presentation.Interfaces;

public interface IOrderService
{
    Task<OrderEntity> CreateOrder(OrderDto form, int customerId);
    Task<IEnumerable<OrderEntity>> GetAllOrders();
    Task<OrderEntity> GetOrderById(int id);
    Task<OrderEntity> UpdateOrder(int id, OrderEntity updatedOrder);
    Task<bool> DeleteOrder(int id);
}
