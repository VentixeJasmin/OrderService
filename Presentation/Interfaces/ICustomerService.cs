using Data.Entities;
using Presentation.Models;

namespace Presentation.Interfaces;

public interface ICustomerService
{
    Task<CustomerEntity> CreateCustomer(OrderDto form);
    Task<IEnumerable<CustomerEntity>> GetAllCustomers();
    Task<CustomerEntity> GetCustomerById(int id);
    Task<CustomerEntity> GetCustomerByEmail(string email);
    Task<CustomerEntity> UpdateCustomer(int id, CustomerEntity updatedCustomer);
    Task<CustomerEntity> UpdateCustomerFromOrder(int customerId, OrderDto orderDto);
    Task<bool> DeleteCustomer(int id);
}
