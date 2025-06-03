using Data.Entities;
using Presentation.Models;

namespace Presentation.Interfaces;

public interface ICustomerService
{
    Task<CustomerEntity> CreateCustomer(OrderDto form);
    Task<IEnumerable<CustomerEntity>> GetAllCustomers();
    Task<CustomerEntity> GetCustomerById(int id);
    Task<CustomerEntity> UpdateCustomer(int id, CustomerEntity updatedCustomer);
    Task<bool> DeleteCustomer(int id);
}
