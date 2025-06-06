﻿using Data.Entities;
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

        public async Task<CustomerEntity> GetCustomerByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return null!;

                return await _customerRepository.GetAsync(c => c.Email == email);

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

        //Whole method generated by Claude AI: 
        public async Task<CustomerEntity> UpdateCustomerFromOrder(int customerId, OrderDto orderDto)
        {
            try
            {
                var existingCustomer = await _customerRepository.GetAsync(c => c.Id == customerId);
                if (existingCustomer == null)
                    return null!;

                // Update customer information with data from the new order
                // Only update if the new data is not empty/null
                if (!string.IsNullOrEmpty(orderDto.FirstName))
                    existingCustomer.FirstName = orderDto.FirstName;

                if (!string.IsNullOrEmpty(orderDto.LastName))
                    existingCustomer.LastName = orderDto.LastName;

                if (!string.IsNullOrEmpty(orderDto.PhoneNumber))
                    existingCustomer.PhoneNumber = orderDto.PhoneNumber;

                if (!string.IsNullOrEmpty(orderDto.StreetAddress))
                    existingCustomer.StreetAddress = orderDto.StreetAddress;

                if (!string.IsNullOrEmpty(orderDto.PostCode))
                    existingCustomer.PostCode = orderDto.PostCode;

                if (!string.IsNullOrEmpty(orderDto.City))
                    existingCustomer.City = orderDto.City;

                // For payment info, only update if using credit card payment
                if (orderDto.PaymentMethodId == 1 && !string.IsNullOrEmpty(orderDto.CreditCardNumber))
                {
                    existingCustomer.LastFourDigits = orderDto.CreditCardNumber.Substring(Math.Max(0, orderDto.CreditCardNumber.Length - 4));
                    // Generate new transaction ID for new payment method
                    existingCustomer.PaymentTransactionId = Guid.NewGuid().ToString();
                }

                var updatedCustomer = await _customerRepository.UpdateAsync(c => c.Id == customerId, existingCustomer);
                await _customerRepository.SaveAsync();

                return updatedCustomer;
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
