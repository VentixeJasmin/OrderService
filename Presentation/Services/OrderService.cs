using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.Interfaces;
using Presentation.Models;

namespace Presentation.Services; 

public class OrderService(IOrderRepository orderRepository) : IOrderService
{   
    private readonly IOrderRepository _orderRepository = orderRepository;
    
    public async Task<OrderEntity> CreateOrder(OrderDto form, int customerId)
    {
        try
        {
            if (form == null)
                return null!;  

            OrderEntity order = new OrderEntity
            {
                EventId = form.EventId,
                Quantity = form.Quantity,
                PricePerTicket = form.PricePerTicket,
                PaymentMethodId = form.PaymentMethodId,
                IsPaid = form.PaymentMethodId == 1 || form.PaymentMethodId == 2,
                CustomerId = customerId
            };
            
            var orderResult = await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveAsync();

            if (orderResult == null)
            {
                return null!;
            }

            return orderResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null!;
        }
    }

    public async Task<IEnumerable<OrderEntity>> GetAllOrders()
    {
        try
        {
            return await _orderRepository.GetAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null!;
        }
    }

    public async Task<OrderEntity> GetOrderById(int id)
    {
        try
        {
            if (id <= 0)
                return null!;

            return await _orderRepository.GetAsync(o => o.Id == id);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null!;
        }
    }

    public async Task<OrderEntity> UpdateOrder(int id, OrderEntity updatedOrder)
    {
        try
        {
            if (id <= 0)
                return null!;

            return await _orderRepository.UpdateAsync(o => o.Id == id, updatedOrder);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null!;
        }
    }

    public async Task<bool> DeleteOrder(int id)
    {
        try
        {
            if (id <= 0)
                return false;

            return await _orderRepository.DeleteAsync(o => o.Id == id);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
}
