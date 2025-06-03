using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.IdentityModel.Tokens;
using Presentation.Interfaces;
using Presentation.Models;

namespace Presentation.Services
{
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<CustomerEntity?> CreateCustomer(OrderDto form)
        {
            if (form == null)
                return null!;

            CustomerEntity customer = new()
            {
                Email = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName,
                PhoneNumber = form.PhoneNumber ?? "",
                StreetAddress = form.StreetAddress ?? "",
                PostCode = form.PostCode ?? "",
                City = form.City ?? "",
                PaymentTransactionId = Guid.NewGuid().ToString(),
                LastFourDigits = form.CreditCardNumber?.Substring(Math.Max(0, form.CreditCardNumber.Length - 4)) ?? ""
            };

            var customerResult = await _customerRepository.CreateAsync(customer);
            await _customerRepository.SaveAsync();

            if (customerResult == null)
            {
                return null!;
            }

            return customerResult;
        }



        public async Task<IEnumerable<CustomerEntity>> GetAllCustomers()
        {
            try
            {
                return await _customerRepository.GetAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null!;
            }
        }

        public async Task<CustomerEntity> GetCustomerById(int id)
        {
            try
            {
                if (id <= 0)
                    return null!;

                return await _customerRepository.GetAsync(c => c.Id == id);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null!;
            }
        }

        public async Task<CustomerEntity> UpdateCustomer(int id, CustomerEntity updatedCustomer)
        {
            try
            {
                if (id <= 0)
                    return null!;

                return await _customerRepository.UpdateAsync(c => c.Id == id, updatedCustomer);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null!;
            }
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            try
            {
                if (id <= 0)
                    return false;

                return await _customerRepository.DeleteAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }    
}
